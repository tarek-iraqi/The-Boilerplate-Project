using Helpers.Classes;
using System.Linq;

namespace Helpers.Interfaces
{
    public interface ISpecificationEvaluator<T> where T : class
    {
        IQueryable<TResult> GetQuery<TResult>(IQueryable<T> inputQuery, Specification<T, TResult> specification);
        IQueryable<T> GetQuery(IQueryable<T> inputQuery, Specification<T> specification);
    }
}
