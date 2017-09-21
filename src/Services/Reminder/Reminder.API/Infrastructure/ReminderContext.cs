using Microsoft.EntityFrameworkCore;
using Reminder.API.Models.Entities;

namespace Reminder.API.Infrastructure
{
    public class ReminderContext : DbContext
    {
        public ReminderContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<ReminderItem> ReminderItems { get; set; }
    }
}