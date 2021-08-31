using Helpers.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Interfaces
{
    public interface ISpecificationEvaluator<T> where T : class
    {
        IQueryable<TResult> GetQuery<TResult>(IQueryable<T> inputQuery, Specification<T, TResult> specification);
        IQueryable<T> GetQuery(IQueryable<T> inputQuery, Specification<T> specification);
    }
}
