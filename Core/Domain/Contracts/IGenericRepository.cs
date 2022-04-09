using Helpers.Classes;
using Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Entity();
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(object id);
        Task<T> GetBySpecAsync(Specification<T> specification);
        Task<TResult> GetBySpecAsync<TResult>(Specification<T, TResult> specification);
        Task<IEnumerable<T>> ListAsync();
        Task<IEnumerable<T>> ListAsync(Specification<T> specification);
        Task<IEnumerable<TResult>> ListAsync<TResult>(Specification<T, TResult> specification);
        PaginatedResult<TResult> PaginatedList<TResult>(int pageNumber, int pageSize,
            Expression<Func<T, TResult>> mapping,
            Expression<Func<T, bool>> expression = null);
        PaginatedResult<TResult> PaginatedList<TResult>(Specification<T, TResult> specification,
            int pageNumber, int pageSize);
        Task<int> CountAsync();
        Task<int> CountAsync(Specification<T> specification);
    }
}
