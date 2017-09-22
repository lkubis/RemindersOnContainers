using System;
using System.Threading.Tasks;
using Audit.API.Infrastructure.Repositories;
using Audit.API.IntegrationEvents.Events;
using Audit.API.Models;
using EventBus.Abstractions;
using Newtonsoft.Json;

namespace Audit.API.IntegrationEvents.EventHandling
{
    public class ReminderCreatedIntegrationEventHandler : IIntegrationEventHandler<ReminderCreatedIntegrationEvent>
    {
        private readonly IMessageRepository _repository;

        public ReminderCreatedIntegrationEventHandler(IMessageRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(ReminderCreatedIntegrationEvent @event)
        {
            await _repository.AddMessage(new Message()
            {
                Title = "ReminderCreated",
                RawData = JsonConvert.SerializeObject(@event),
                CreatedBy = @event.CreatedBy,
                CreationDate = DateTimeOffset.UtcNow
            });
        }
    }
}