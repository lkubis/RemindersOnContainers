using System;
using EventBus.Events;

namespace Audit.API.IntegrationEvents.Events
{
    public class ReminderCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid ReminderId { get; }
        public string Title { get; }
        public string Notes { get; }
        public DateTimeOffset Date { get; }
        public string CreatedBy { get; }

        public ReminderCreatedIntegrationEvent(Guid id, string title, string notes, DateTimeOffset date, string createdBy)
        {
            ReminderId = id;
            Title = title;
            Notes = notes;
            Date = date;
            CreatedBy = createdBy;
        }
    }
}