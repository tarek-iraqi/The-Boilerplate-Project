﻿using System;
using System.Threading.Tasks;

namespace Helpers.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

        Task<int> CompleteAsync();
    }
}
