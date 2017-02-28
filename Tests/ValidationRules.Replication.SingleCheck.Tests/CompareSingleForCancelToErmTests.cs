﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using LinqToDB.Data;

using NuClear.ValidationRules.Storage;
using NuClear.ValidationRules.Storage.Model.Messages;

using NUnit.Framework;

using ValidationRules.Replication.SingleCheck.Tests.ErmService;

using ValidationRules.Replication.SingleCheck.Tests.RiverService;

using Version = NuClear.ValidationRules.Storage.Model.Messages.Version;

namespace ValidationRules.Replication.SingleCheck.Tests
{
    [TestFixture]
    public sealed class CompareSingleForCancelToErmTests
    {
        private const int OrdersPerRule = 30;

        private static readonly Expression<Func<Version.ValidationResult, bool>> IsApplicableForSingleCheck = x => (x.Result & (0x3 << 10)) > 0;

        private readonly RiverToErmResultAdapter _riverService = new RiverToErmResultAdapter("River");
        private readonly ErmToRiverResultAdapter _ermService = new ErmToRiverResultAdapter("Erm");

        public IReadOnlyCollection<TestCaseData> Rules
        {
            get
            {
                var rules = Enum.GetValues(typeof(MessageTypeCode))
                                .Cast<int>()
                                .GroupBy(x => x.ToErmRuleCode())
                                .ToDictionary(x => x.Key, x => x.ToArray());

                var result = new List<TestCaseData>(rules.Count);

                using (var dc = new DataConnection("ReferenceSource").AddMappingSchema(Schema.Messages))
                {
                    var orderErrors = dc.GetTable<Version.ValidationResult>().Where(x => x.Resolved == false && x.OrderId.HasValue).Where(IsApplicableForSingleCheck);
                    var resolved = dc.GetTable<Version.ValidationResult>().Where(x => x.Resolved == true);
                    var results =
                        from message in orderErrors
                        where !resolved.Any(x => x.MessageType == message.MessageType && x.OrderId == message.OrderId && x.VersionId > message.VersionId)
                        select message;

                    foreach (var rule in rules)
                    {
                        var orderIds = results.OrderBy(x => x.OrderId)
                                              .Where(x => rule.Value.Contains(x.MessageType) && x.OrderId.HasValue)
                                              .Select(x => x.OrderId.Value)
                                              .Distinct()
                                              .Take(OrdersPerRule)
                                              .ToArray();

                        if (!orderIds.Any())
                        {
                            result.Add(new TestCaseData(rule.Key, new Int32Collection(rule.Value), null));
                        }

                        foreach (var orderId in orderIds)
                        {
                            result.Add(new TestCaseData(rule.Key, new Int32Collection(rule.Value), orderId));
                        }
                    }
                }

                return result;
            }
        }

        [TestCaseSource(nameof(Rules))]
        public void TestRule(int ermRule, Int32Collection riverRules, long? orderId)
        {
            if (!orderId.HasValue)
            {
                Assert.Inconclusive();
            }

            var riverTime = Stopwatch.StartNew();
            var riverResult = InvokeRiver(orderId.Value, riverRules.Values).SelectMany(x => x.Value).ToArray();
            riverTime.Stop();

            var ermTime = Stopwatch.StartNew();
            var ermResult = InvokeErm(orderId.Value, ermRule).SelectMany(x => x.Value).ToArray();
            ermTime.Stop();

            var onlyRiver = riverResult.Except(ermResult).ToArray();
            var onlyErm = ermResult.Except(riverResult).ToArray();
            var common = ermResult.Intersect(riverResult).ToArray();

            if (onlyRiver.Any() || onlyErm.Any())
            {
                Assert.Fail($"River messages ({onlyRiver.Length} of {riverResult.Length}):\n" +
                            $"{string.Join(Environment.NewLine, onlyRiver)}\n" +
                            $"Erm messages ({onlyErm.Length} of {ermResult.Length}):\n" +
                            $"{string.Join(Environment.NewLine, onlyErm)}\n" +
                            $"Common ({common.Length}):\n" +
                            $"{string.Join(Environment.NewLine, common)}");
            }

            Assert.Pass($"River: {riverTime.ElapsedMilliseconds}, Erm: {ermTime.ElapsedMilliseconds}");
        }

        public IReadOnlyCollection<long> Orders
            => Array.Empty<long>();

        [TestCaseSource(nameof(Orders))]
        public void TestOrder(long orderId)
        {
            var riverResult = InvokeRiver(orderId);
            var ermResult = InvokeErm(orderId);

            var fails = new Dictionary<int, string>();
            foreach (var key in riverResult.Keys.Union(ermResult.Keys))
            {
                var rr = TryGet(riverResult, key);
                var er = TryGet(ermResult, key);

                var onlyRiver = rr.Except(er).ToArray();
                var onlyErm = er.Except(rr).ToArray();
                var common = er.Intersect(rr).ToArray();

                if (onlyRiver.Any() || onlyErm.Any())
                {
                    fails.Add(
                        key,
                        $"River messages ({onlyRiver.Length} of {rr.Length}):\n" +
                        $"{string.Join(Environment.NewLine, onlyRiver)}\n\n" +
                        $"Erm messages ({onlyErm.Length} of {er.Length}):\n" +
                        $"{string.Join(Environment.NewLine, onlyErm)}\n\n" +
                        $"Common ({common.Length}):\n" +
                        $"{string.Join(Environment.NewLine, common)}\n");
                }
            }

            if (fails.Any())
            {
                Assert.Fail(string.Join("\n=========\n", fails.Select(x => $"{x.Key}\n{x.Value}")));
            }
        }

        private IDictionary<int, string[]> InvokeRiver(long orderId, int[] rules = null)
            => _riverService.ValidateSingleForCancel(orderId).Messages
                            .Where(x => rules == null || rules.Contains(x.RuleCode))
                            .GroupBy(x => x.RuleCode.ToErmRuleCode(), x => x.MessageText.TrimEnd('.'))
                            .ToDictionary(x => x.Key, x => x.OrderBy(y => y).ToArray());

        private IDictionary<int, string[]> InvokeErm(long orderId, int? rule = null)
            => _ermService.ValidateSingleForCancel(orderId)
                          .Where(x => rule == null || x.RuleCode == rule)
                          .GroupBy(x => x.RuleCode, x => x.MessageText.TrimEnd('.'))
                          .ToDictionary(x => x.Key, x => x.OrderBy(y => y).ToArray());

        private string[] TryGet(IDictionary<int, string[]> result, int key)
        {
            string[] value;
            result.TryGetValue(key, out value);
            return value ?? Array.Empty<string>();
        }

        // Для красивого отображения в списке тестов вместо непонятного Int32[]
        public class Int32Collection
        {
            public Int32Collection(int[] values)
            {
                Values = values;
            }

            public int[] Values { get; }

            public override string ToString()
            {
                return "[" + string.Join(", ", Values) + "]";
            }
        }
    }
}
