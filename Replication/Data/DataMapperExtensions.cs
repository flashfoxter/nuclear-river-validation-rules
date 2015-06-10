﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using LinqToDB.Expressions;

using NuClear.AdvancedSearch.Replication.Model;

namespace NuClear.AdvancedSearch.Replication.Data
{
    internal static class DataMapperExtensions
    {
        private static readonly MethodInfo InsertMethodInfo = ((MethodInfo)MemberHelper.GetMemeberInfo((Expression<Action<IDataMapper>>)(mapper => mapper.Insert(default(IIdentifiable))))).GetGenericMethodDefinition();
        private static readonly MethodInfo UpdateMethodInfo = ((MethodInfo)MemberHelper.GetMemeberInfo((Expression<Action<IDataMapper>>)(mapper => mapper.Update(default(IIdentifiable))))).GetGenericMethodDefinition();
        private static readonly MethodInfo DeleteMethodInfo = ((MethodInfo)MemberHelper.GetMemeberInfo((Expression<Action<IDataMapper>>)(mapper => mapper.Delete(default(IIdentifiable))))).GetGenericMethodDefinition();

        private static readonly ConcurrentDictionary<Type, MethodInfo> InsertMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> UpdateMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> DeleteMethods = new ConcurrentDictionary<Type, MethodInfo>();

        public static void InsertAll(this IDataMapper mapper, IQueryable query)
        {
            InvokeMethodOn(ResolveMethod(InsertMethods, InsertMethodInfo, query.ElementType), mapper, query);
        }

        public static void UpdateAll(this IDataMapper mapper, IQueryable query)
        {
            InvokeMethodOn(ResolveMethod(UpdateMethods, UpdateMethodInfo, query.ElementType), mapper, query);
        }

        public static void DeleteAll(this IDataMapper mapper, IQueryable query)
        {
            var items = query.Cast<IObject>().ToArray();
            InvokeMethodOn(ResolveMethod(DeleteMethods, DeleteMethodInfo, query.ElementType), mapper, items);
        }

        private static void InvokeMethodOn(MethodInfo method, IDataMapper mapper, IEnumerable items)
        {
            foreach (var item in items)
            {
                method.Invoke(mapper, new[] { item });
            }
        }

        private static MethodInfo ResolveMethod(ConcurrentDictionary<Type, MethodInfo> methods, MethodInfo definition, Type type)
        {
            return methods.GetOrAdd(type, t => definition.MakeGenericMethod(t));
        }
    }
}