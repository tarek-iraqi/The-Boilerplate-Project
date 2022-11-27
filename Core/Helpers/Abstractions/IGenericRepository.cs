using Helpers.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Helpers.Abstractions;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Entity();
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    Task<T> GetByIdAsync(object id, CancellationToken cancellationToken = default);
    Task<T> GetBySpecAsync(Specification<T> specification, CancellationToken cancellationToken = default);
    Task<TResult> GetBySpecAsync<TResult>(Specification<T, TResult> specification,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> ListAsync(Specification<T> specification, CancellationToken cancellationToken = default);
    Task<IEnumerable<TResult>> ListAsync<TResult>(Specification<T, TResult> specification,
        CancellationToken cancellationToken = default);
    Task<PaginatedResult<TResult>> PaginatedListAsync<TResult>(int pageNumber, int pageSize,
        Expression<Func<T, TResult>> mapping,
        Expression<Func<T, bool>> expression = null,
        CancellationToken cancellationToken = default);
    Task<PaginatedResult<TResult>> PaginatedListAsync<TResult>(Specification<T, TResult> specification,
        int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(Specification<T> specification, CancellationToken cancellationToken = default);
}
