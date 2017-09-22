using System.Threading.Tasks;
using EventBus.Abstractions;
using Reminder.API.IntegrationEvents.Events;

namespace Reminder.API.IntegrationEvents.EventHandling
{
    // TODO: Move to Audit microservice
    public class ReminderCreatedIntegrationEventHandler : IIntegrationEventHandler<ReminderCreatedIntegrationEvent>
    {
        public Task Handle(ReminderCreatedIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}