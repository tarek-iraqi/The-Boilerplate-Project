using Application.Contracts;
using Domain.Entities;
using Helpers.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services;

public class BackgroundCronJobs : IBackgroundCronJobs
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public BackgroundCronJobs(IUnitOfWork unitOfWork,
        IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }
    public async Task HandleDomainEvents()
    {
        var outboxMessages = await _unitOfWork.Repository<OutboxMessage>()
            .Entity()
            .Where(x => !x.ProcessedOnUtc.HasValue)
            .ToListAsync();

        foreach (var message in outboxMessages)
        {
            var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });

            if (domainEvent is null) continue;

            try
            {
                await _publisher.Publish(domainEvent);
                message.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                message.Error = ex.Message;
            }
        }

        await _unitOfWork.CompleteAsync();
    }
}