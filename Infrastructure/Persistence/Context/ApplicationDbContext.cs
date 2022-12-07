using Application.Contracts;
using Domain.Entities;
using Helpers.Abstractions;
using Helpers.Extensions;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence.AuditTrail;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Context;

public class ApplicationDbContext : AuditableIdentityContext, IDataProtectionKeyContext
{
    private readonly IAuthenticatedUserService _authenticatedUser;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IAuthenticatedUserService authenticatedUser)
        : base(options)
    {
        _authenticatedUser = authenticatedUser;
    }
    public DbSet<Device> Devices { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public IDbConnection Connection => Database.GetDbConnection();

    public bool HasChanges => ChangeTracker.HasChanges();

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        MetaDataHandler();

        return await base.SaveChangesAsync(_authenticatedUser.UserId);
    }

    private void MetaDataHandler()
    {
        var now = DateTimeExtensions.SystemDateTimeNow;

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is IBaseEntity entity)
            {
                switch (changedEntity.State)
                {
                    case EntityState.Added:
                        entity.CreatedOn = now;
                        entity.CreatedBy = _authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        Entry(entity).Property(x => x.CreatedOn).IsModified = false;
                        entity.LastModifiedOn = now;
                        entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                    case EntityState.Deleted:
                        Entry(entity).State = EntityState.Modified;
                        entity.LastModifiedOn = now;
                        entity.LastModifiedBy = _authenticatedUser.UserId;
                        entity.IsDeleted = true;
                        foreach (var navigationEntry in changedEntity.Navigations)
                        {
                            if (navigationEntry is INavigation n && !n.IsOnDependent)
                            {
                                if (navigationEntry is CollectionEntry collectionEntry)
                                {
                                    foreach (var dependentEntry in collectionEntry.CurrentValue)
                                    {
                                        HandleDependent(Entry(dependentEntry));
                                    }
                                }
                                else
                                {
                                    var dependentEntry = navigationEntry.CurrentValue;
                                    if (dependentEntry != null)
                                    {
                                        HandleDependent(Entry(dependentEntry));
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }
    }

    private void HandleDependent(EntityEntry entry)
    {
        if (entry is IBaseEntity navEntry)
            navEntry.IsDeleted = true;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var property in builder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(10,2)");
        }

        foreach (var fk in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            fk.DeleteBehavior = DeleteBehavior.NoAction;
        }

        builder.ApplyGlobalIgnore<IBaseEntity>("DomainEvents");

        #region Global Query Filters

        builder.ApplyGlobalFilters<IBaseEntity>(e => !e.IsDeleted);

        #endregion

        #region Entities Configuration
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        #endregion       
    }
}