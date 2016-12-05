using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.List;
using static LanguageExt.Prelude;

namespace WalnutBrain.Showcase
{
    public class ShowcaseSet : IShowcaseSet
    {
        private class DataPropertyInfo
        {
            public DataPropertyInfo(Type type)
            {
                Type = type;
            }

            public Type Type { get; }
        }

        private _pointPropertiesLock = 
        private Map<string, DataPropertyInfo> _pointProperties = LanguageExt.Map<string, DataPropertyInfo>.Empty;

        private Option<PropertyInfo> FindPropertyByName(IReadOnlyCollection<PropertyInfo> properties, Type propertyType, IEnumerable<string> propertyNames)
        {
            foreach (var name in propertyNames)
            {
                var pi = properties.FirstOrDefault(p => p.Name == name && p.PropertyType == propertyType);
                if (pi != null) return Some(pi);
            }
            return None;
        }

        public Task PublishPoints<T>(IEnumerable<T> points, Expression<Func<T, string>> idSelector = null, Func<T, bool> slicer = null)
        {
            var properties = createRange(typeof(T).GetRuntimeProperties());
            var idProperty =
                idSelector == null
                    ? FindPropertyByName(properties, typeof(string),
                            create("Id", "ID", "PointId", "PointID", typeof(T).Name + "Id", typeof(T).Name + "ID"))
                        .Match(p => p, () => { throw new InvalidOperationException("Id property not found"); })
                    : Tools.TryGetPropertyInfo(idSelector)
                        .Match(p => properties.First(t => t.Name == p.Name), () => { throw new InvalidOperationException("Invalid id property selector"); });
            var dataProperties = remove(properties, idProperty);
            if(_pointProperties.Keys.Any(p => dataProperties.Any(t => t.Name == p )))
        }

        public Task PublishPointData<TData, TSlice>(IEnumerable<TData> data, Expression<Func<TData, string>> idSelector = null, IEnumerable<Expression<Func<TData, object>>> includeSelector = null,
            IEnumerable<Expression<Func<TData, object>>> excludeSelector = null, Func<TSlice, bool> slicer = null)
        {
            throw new NotImplementedException();
        }
    }
}