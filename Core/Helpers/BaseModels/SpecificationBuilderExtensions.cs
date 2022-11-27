using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Helpers.BaseModels
{
    public static class SpecificationBuilderExtensions
    {
        public static Specification<T> Where<T>(
            this Specification<T> specification,
            Expression<Func<T, bool>> criteria)
        {
            ((List<Expression<Func<T, bool>>>)specification.WhereExpressions).Add(criteria);

            return specification;
        }

        public static Specification<T> OrderBy<T>(
            this Specification<T> specification,
            Expression<Func<T, object>> orderExpression)
        {
            ((List<(Expression<Func<T, object>> OrderExpression, OrderType OrderType)>)specification.OrderExpressions)
                .Add((orderExpression, OrderType.OrderBy));

            return specification;
        }

        public static Specification<T> ThenBy<T>(
            this Specification<T> specification,
            Expression<Func<T, object>> orderExpression)
        {
            ((List<(Expression<Func<T, object>> OrderExpression, OrderType OrderType)>)specification.OrderExpressions)
                .Add((orderExpression, OrderType.ThenBy));

            return specification;
        }

        public static Specification<T> OrderByDescending<T>(
            this Specification<T> specification,
            Expression<Func<T, object>> orderExpression)
        {
            ((List<(Expression<Func<T, object>> OrderExpression, OrderType OrderType)>)specification.OrderExpressions)
                .Add((orderExpression, OrderType.OrderByDescending));

            return specification;
        }

        public static Specification<T> ThenByDescending<T>(
            this Specification<T> specification,
            Expression<Func<T, object>> orderExpression)
        {
            ((List<(Expression<Func<T, object>> OrderExpression, OrderType OrderType)>)specification.OrderExpressions)
                .Add((orderExpression, OrderType.ThenByDescending));

            return specification;
        }

        public static Specification<T> Include<T, TProperty>(
            this Specification<T> specification,
            Expression<Func<T, TProperty>> includeExpression)
        {
            var aggregator = new IncludeAggregator((includeExpression.Body as MemberExpression)?.Member?.Name);

            ((List<IncludeAggregator>)specification.IncludeAggregators).Add(aggregator);
            return specification;
        }

        public static Specification<T> Include<T>(
            this Specification<T> specification,
            string includeString)
        {
            ((List<string>)specification.IncludeStrings).Add(includeString);
            return specification;
        }

        public static Specification<T> Take<T>(
            this Specification<T> specification,
            int take)
        {
            if (specification.Take != null)
                throw new DuplicateWaitObjectException($"Duplicate Take Exception, {specification.Take}");

            specification.Take = take;
            return specification;
        }

        public static Specification<T> Skip<T>(
            this Specification<T> specification,
            int skip)
        {
            if (specification.Skip != null)
                throw new DuplicateWaitObjectException($"Duplicate Skip Exception, {specification.Skip}");

            specification.Skip = skip;
            return specification;
        }

        public static Specification<T, TResult> Select<T, TResult>(
            this Specification<T, TResult> specification,
            Expression<Func<T, TResult>> selector)
        {
            specification.Selector = selector;

            return specification;
        }
    }
}
