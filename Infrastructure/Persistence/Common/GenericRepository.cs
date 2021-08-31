using Helpers.Classes;
using Helpers.Extensions;
using Helpers.Interfaces;
using Helpers.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
        }

        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetBySpecAsync(Specification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<TResult> GetBySpecAsync<TResult>(Specification<T, TResult> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ListAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }           

        public async Task<IEnumerable<T>> ListAsync(Specification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<IEnumerable<TResult>> ListAsync<TResult>(Specification<T, TResult> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public PaginatedResult<TResult> PaginatedList<TResult>(Specification<T, TResult> specification,
            int pageNumber, int pageSize)
        {
            return ApplySpecification(specification).ToPaginatedList(pageNumber, pageSize);
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(Specification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        protected IQueryable<T> ApplySpecification(Specification<T> specification)
        {
            return specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
        }
        protected IQueryable<TResult> ApplySpecification<TResult>(Specification<T, TResult> specification)
        {
            if (specification is null) throw new ArgumentNullException("specification", "Specification is required");
            if (specification.Selector is null) throw new ArgumentException("Selector Not Found Exception");

            return specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
        }
    }
}
