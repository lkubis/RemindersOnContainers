using System;
using EventBus.Events;

namespace Reminder.API.IntegrationEvents.Events
{
    public class ReminderCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid ReminderId { get; }
        public string Title { get; }
        public string Notes { get; }
        public DateTimeOffset Date { get; }

        public ReminderCreatedIntegrationEvent(Guid id, string title, string notes, DateTimeOffset date)
        {
            ReminderId = id;
            Title = title;
            Notes = notes;
            Date = date;
        }
    }
}