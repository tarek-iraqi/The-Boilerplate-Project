using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly ISpecificationEvaluator<T> specificationEvaluator;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            specificationEvaluator = new SpecificationEvaluator<T>();
        }

        public IQueryable<T> Entity() => _dbContext.Set<T>();

        public void Add(T entity) => _dbContext.Set<T>().Add(entity);

        public void AddRange(IEnumerable<T> entities) => _dbContext.Set<T>().AddRange(entities);

        public void Update(T entity) => _dbContext.Entry(entity).CurrentValues.SetValues(entity);

        public void Remove(T entity) => _dbContext.Set<T>().Remove(entity);

        public void RemoveRange(IEnumerable<T> entities) => _dbContext.Set<T>().RemoveRange(entities);

        public async Task<T> GetByIdAsync(object id, CancellationToken cancellationToken = default) =>
            await _dbContext.Set<T>().FindAsync(id, cancellationToken);

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression) => _dbContext.Set<T>().Where(expression);

        public async Task<T> GetBySpecAsync(Specification<T> specification, CancellationToken cancellationToken = default)
            => await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);

        public async Task<TResult> GetBySpecAsync<TResult>(Specification<T, TResult> specification,
            CancellationToken cancellationToken = default)
            => await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default) =>
            await _dbContext.Set<T>().ToListAsync(cancellationToken);

        public async Task<IEnumerable<T>> ListAsync(Specification<T> specification, CancellationToken cancellationToken = default)
            => await ApplySpecification(specification).ToListAsync(cancellationToken);

        public async Task<IEnumerable<TResult>> ListAsync<TResult>(Specification<T, TResult> specification,
            CancellationToken cancellationToken = default)
            => await ApplySpecification(specification).ToListAsync(cancellationToken);

        public async Task<PaginatedResult<TResult>> PaginatedListAsync<TResult>(int pageNumber, int pageSize,
            Expression<Func<T, TResult>> mapping, Expression<Func<T, bool>> expression = null,
            CancellationToken cancellationToken = default)
            => expression == null
                ? await _dbContext.Set<T>().Where(expression).Select(mapping).ToPaginatedListAsync(pageNumber, pageSize)
                : await _dbContext.Set<T>().Select(mapping).ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

        public async Task<PaginatedResult<TResult>> PaginatedListAsync<TResult>(Specification<T, TResult> specification,
            int pageNumber, int pageSize, CancellationToken cancellationToken = default)
            => await ApplySpecification(specification).ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

        public async Task<int> CountAsync(CancellationToken cancellationToken = default) =>
            await _dbContext.Set<T>().CountAsync(cancellationToken);

        public async Task<int> CountAsync(Specification<T> specification, CancellationToken cancellationToken = default) =>
            await ApplySpecification(specification).CountAsync(cancellationToken);

        protected IQueryable<T> ApplySpecification(Specification<T> specification)
        {
            return specification is null
                ? throw new ArgumentNullException("specification", "Specification is required")
                : specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
        }

        protected IQueryable<TResult> ApplySpecification<TResult>(Specification<T, TResult> specification)
        {
            if (specification is null) throw new ArgumentNullException("specification", "Specification is required");
            if (specification.Selector is null) throw new ArgumentException("Selector Not Found Exception");

            return specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
        }
    }
}
