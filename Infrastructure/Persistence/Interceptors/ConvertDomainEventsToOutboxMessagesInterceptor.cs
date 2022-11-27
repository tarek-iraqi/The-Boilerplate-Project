using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Interceptors
{
    internal class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            DbContext context = eventData.Context;

            if (context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            SaveDomainEvents(context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DbContext context = eventData.Context;

            if (context is null) return base.SavingChanges(eventData, result);

            SaveDomainEvents(context);

            return base.SavingChanges(eventData, result);
        }

        private static void SaveDomainEvents(DbContext context)
        {
            var outboxMessages = context.ChangeTracker
                .Entries<IDomainEventCollection>()
                .Select(x => x.Entity)
                .Where(x => x.DomainEvents is not null)
                .SelectMany(x => x.DispatchEvents())
                .Select(x => new OutboxMessage
                {
                    OccurredOnUtc = DateTime.UtcNow,
                    Type = x.GetType().Name,
                    Content = JsonConvert.SerializeObject(x, new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
                }).ToList();

            context.Set<OutboxMessage>().AddRange(outboxMessages);
        }
    }
}
