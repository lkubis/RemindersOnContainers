using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Reminder.API.Infrastructure
{
    public class DbInitializer
    {
        private readonly ReminderContext _context;

        public DbInitializer(ReminderContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();
        }
    }
}