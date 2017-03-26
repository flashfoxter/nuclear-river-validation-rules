﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

using LinqToDB.Data;

using NuClear.ValidationRules.SingleCheck;
using NuClear.ValidationRules.Storage;
using NuClear.ValidationRules.Storage.Model.Messages;

using NUnit.Framework;

using Version = NuClear.ValidationRules.Storage.Model.Messages.Version;

namespace ValidationRules.Replication.Comparison.Tests
{
    [TestFixture]
    public sealed class SingleToMassTests
    {
        private const int OrderPerRule = 1;

        private IEnumerable<MessageTypeCode> Rules
            => Enum.GetValues(typeof(MessageTypeCode)).Cast<MessageTypeCode>();

        [TestCaseSource(nameof(Rules))]
        public void TestRule(MessageTypeCode rule)
        {
            var results = GetResultsFromBigDatabase(x => x.MessageType == (int)rule && x.OrderId.HasValue);
            if (!results.Any())
            {
                Assert.Inconclusive();
            }

            var expectedResultsByOrder = results.GroupBy(x => x.OrderId.Value, x => x).Take(OrderPerRule).ToArray();

            var validator = new ValidatorFactory().Create();
            foreach (var expected in expectedResultsByOrder)
            {
                var actual = validator.Execute(expected.Key).Where(x => x.MessageType == (int)rule).ToArray();

                AssertCollectionsEqual(MergePeriods(expected), MergePeriods(actual));
            }
        }

        private IEnumerable<long> Orders
            => Array.Empty<long>();

        [TestCaseSource(nameof(Orders))]
        public void TestOrder(long orderId)
        {
            var expected = GetResultsFromBigDatabase(x => x.OrderId == orderId);

            var validator = new ValidatorFactory().Create();
            var actual = validator.Execute(orderId);

            AssertCollectionsEqual(MergePeriods(expected), MergePeriods(actual));
        }

        private IReadOnlyCollection<Version.ValidationResult> GetResultsFromBigDatabase(Expression<Func<Version.ValidationResult, bool>> filter)
        {
            using (var dc = new DataConnection("Messages").AddMappingSchema(Schema.Messages))
            {
                var orderErrors = dc.GetTable<Version.ValidationResult>().Where(x => x.Resolved == false && x.OrderId.HasValue);
                var resolved = dc.GetTable<Version.ValidationResult>().Where(x => x.Resolved == true);

                var results =
                    from result in orderErrors.Where(filter)
                    where !resolved.Any(x => x.MessageType == result.MessageType && x.OrderId == result.OrderId && x.VersionId > result.VersionId)
                    select result;

                return results.ToArray();
            }
        }

        private IReadOnlyDictionary<XNode, List<Tuple<DateTime, DateTime>>> MergePeriods(IEnumerable<Version.ValidationResult> results)
            => results.GroupBy(x => new XElement("rule", new XAttribute("id", x.MessageType), x.MessageParams.Root), x => Tuple.Create(x.PeriodStart, x.PeriodEnd), XNode.EqualityComparer)
                      .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Item1).Aggregate(new List<Tuple<DateTime, DateTime>>(), AppendPeriod), XNode.EqualityComparer);

        private List<Tuple<DateTime, DateTime>> AppendPeriod(List<Tuple<DateTime, DateTime>> list, Tuple<DateTime, DateTime> period)
        {
            if (!list.Any())
            {
                list.Add(period);
            }
            else
            {
                var last = list.Last();
                if (last.Item2 == period.Item1)
                {
                    list.Remove(last);
                    list.Add(Tuple.Create(last.Item1, period.Item2));
                }
            }

            return list;
        }

        private void AssertCollectionsEqual(IReadOnlyDictionary<XNode, List<Tuple<DateTime, DateTime>>> expected, IReadOnlyDictionary<XNode, List<Tuple<DateTime, DateTime>>> actual)
        {
            var allKeys = expected.Keys.Union(actual.Keys, XNode.EqualityComparer);

            foreach (var key in allKeys)
            {
                List<Tuple<DateTime, DateTime>> leftPeriods;
                if (!expected.TryGetValue(key, out leftPeriods))
                {
                    var keys = string.Join(Environment.NewLine, expected.Keys.Select(x => x.ToString(SaveOptions.DisableFormatting)));
                    Assert.Fail($"one of actual:{key.ToString(SaveOptions.DisableFormatting)}\nis missing in expected:\n {keys}");
                }

                List<Tuple<DateTime, DateTime>> rightPeriods;
                if (!actual.TryGetValue(key, out rightPeriods))
                {
                    var keys = string.Join(Environment.NewLine, actual.Keys.Select(x => x.ToString(SaveOptions.DisableFormatting)));
                    Assert.Fail($"one of expected:{key.ToString(SaveOptions.DisableFormatting)}\nis missing in actual:\n {keys}");
                }

                Assert.AreEqual(leftPeriods, rightPeriods);
            }
        }
    }
}
