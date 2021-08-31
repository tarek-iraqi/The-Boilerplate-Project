using Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Classes
{
    public abstract class Specification<T, TResult> : Specification<T>
    {
        protected new virtual Specification<T, TResult> Query { get; }

        protected Specification() : base()
        {
            Query = this;
        }

        public Expression<Func<T, TResult>> Selector { get; internal set; }
    }

    public abstract class Specification<T>
    {
        protected virtual Specification<T> Query { get; }

        protected Specification()
        {
            Query = this;
        }

        public IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; } = new List<Expression<Func<T, bool>>>();

        public IEnumerable<(Expression<Func<T, object>> KeySelector, OrderType OrderType)> OrderExpressions { get; } =
            new List<(Expression<Func<T, object>> KeySelector, OrderType OrderType)>();

        public IEnumerable<IncludeAggregator> IncludeAggregators { get; } = new List<IncludeAggregator>();

        public IEnumerable<string> IncludeStrings { get; } = new List<string>();

        public Expression<Func<T, object>> GroupBy { get; private set; }

        public int? Take { get; internal set; } = null;

        public int? Skip { get; internal set; } = null;
    }

    public enum OrderType
    {
        OrderBy = 1,
        OrderByDescending = 2,
        ThenBy = 3,
        ThenByDescending = 4
    }
}
