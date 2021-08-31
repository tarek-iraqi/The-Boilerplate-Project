using Helpers.Classes;
using Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Persistence.Common
{
    public class SpecificationEvaluator<T> : ISpecificationEvaluator<T> where T : class
    {
        public virtual IQueryable<TResult> GetQuery<TResult>(IQueryable<T> inputQuery, Specification<T, TResult> specification)
        {
            var query = GetQuery(inputQuery, (Specification<T>)specification);

            var selectQuery = query.Select(specification.Selector);

            return selectQuery;
        }

        public virtual IQueryable<T> GetQuery(IQueryable<T> inputQuery, Specification<T> specification)
        {
            var query = inputQuery;

            foreach (var includeString in specification.IncludeStrings)
            {
                query = query.Include(includeString);
            }

            foreach (var includeAggregator in specification.IncludeAggregators)
            {
                var includeString = includeAggregator.IncludeString;
                if (!string.IsNullOrEmpty(includeString))
                {
                    query = query.Include(includeString);
                }
            }

            foreach (var criteria in specification.WhereExpressions)
            {
                query = query.Where(criteria);
            }


            if (specification.OrderExpressions != null && specification.OrderExpressions.Any())
            {
                IOrderedQueryable<T> orderedQuery = null;
                foreach (var orderExpression in specification.OrderExpressions)
                {
                    if (orderExpression.OrderType == OrderType.OrderBy)
                    {
                        orderedQuery = query.OrderBy(orderExpression.KeySelector);
                    }
                    else if (orderExpression.OrderType == OrderType.OrderByDescending)
                    {
                        orderedQuery = query.OrderByDescending(orderExpression.KeySelector);
                    }
                    else if (orderExpression.OrderType == OrderType.ThenBy)
                    {
                        orderedQuery = orderedQuery.ThenBy(orderExpression.KeySelector);
                    }
                    else if (orderExpression.OrderType == OrderType.ThenByDescending)
                    {
                        orderedQuery = orderedQuery.ThenByDescending(orderExpression.KeySelector);
                    }

                    if (orderedQuery != null)
                    {
                        query = orderedQuery;
                    }
                }
            }

            if (specification.Skip != null && specification.Skip != 0)
            {
                query = query.Skip(specification.Skip.Value);
            }

            if (specification.Take != null)
            {
                query = query.Take(specification.Take.Value);
            }

            return query;
        }
    }
}
