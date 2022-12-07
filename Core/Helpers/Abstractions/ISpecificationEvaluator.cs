using Helpers.BaseModels;
using System.Linq;

namespace Helpers.Abstractions;

public interface ISpecificationEvaluator<T> where T : class
{
    IQueryable<TResult> GetQuery<TResult>(IQueryable<T> inputQuery, Specification<T, TResult> specification);
    IQueryable<T> GetQuery(IQueryable<T> inputQuery, Specification<T> specification);
}