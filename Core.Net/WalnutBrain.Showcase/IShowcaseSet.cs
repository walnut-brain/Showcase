using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WalnutBrain.Showcase
{
    public interface IShowcaseSet
    {
        Task PublishPoints<T>(IEnumerable<T> points, Expression<Func<T, string>> idSelector = null, Func<T, bool> slicer = null);
        Task PublishPointData<TData, TSlice>(IEnumerable<TData> data,
            Expression<Func<TData, string>> idSelector = null,
            IEnumerable<Expression<Func<TData, object>>> includeSelector = null,
            IEnumerable<Expression<Func<TData, object>>> excludeSelector = null,
            Func<TSlice, bool> slicer = null);
    }
}
