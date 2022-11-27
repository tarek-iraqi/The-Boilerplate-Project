using System;
using System.Threading.Tasks;

namespace Helpers.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

    Task<int> CompleteAsync();
}
