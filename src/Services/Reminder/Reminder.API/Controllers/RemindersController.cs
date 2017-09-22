using System;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Abstractions;
using Framework.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reminder.API.Infrastructure;
using Reminder.API.IntegrationEvents.Events;
using Reminder.API.Models.Entities;
using Reminder.API.ViewModels;

namespace Reminder.API.Controllers
{
    [JwtAuthorize]
    [Route("api/v1/[controller]")]
    public class RemindersController : Controller
    {
        #region | Fields

        private readonly ReminderContext _context;
        private readonly IEventBus _eventBus;

        #endregion

        #region | Constructors

        public RemindersController(
            ReminderContext context,
            IEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        #endregion

        #region | Public Methods

        #region | Items - GET: /items[?pageSize={pageSize}&pageIndex={pageIndex}]

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _context.ReminderItems.LongCountAsync();

            var itemsOnPage = await _context.ReminderItems
                .OrderBy(x => x.Date)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<ReminderItem>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        #endregion

        #region | GetItemById - GET: /items/{id}

        [HttpGet]
        [Route("items/{id:guid}")]
        public async Task<IActionResult> GetItemById(Guid id)
        {
            var item = await _context.ReminderItems.FindAsync(id);
            if (item != null)
                return Ok(item);

            return NotFound();
        }

        #endregion

        #region | CreateReminder - POST: /items

        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> CreateReminder([FromBody] ReminderItem reminder)
        {
            var item = new ReminderItem()
            {
                Id = Guid.NewGuid(),
                Title = reminder.Title,
                Notes = reminder.Notes,
                Date = reminder.Date,
                CreationDate = DateTimeOffset.UtcNow
            };
            _context.ReminderItems.Add(item);
            await _context.SaveChangesAsync();

            // Publish integration event
            var @event = new ReminderCreatedIntegrationEvent(item.Id, item.Title, item.Notes, item.Date);
            _eventBus.Publish(@event);

            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, null);
        }

        #endregion

        #region | UpdateReminder - PUT: /items

        [HttpPut]
        [Route("items")]
        public async Task<IActionResult> UpdateReminder([FromBody] ReminderItem reminderToUpdate)
        {
            var item = await _context.ReminderItems.FindAsync(reminderToUpdate.Id);
            if (item == null)
                return NotFound(new { Message = $"Item with id {reminderToUpdate.Id} not found."});

            item.Title = reminderToUpdate.Title;
            item.Notes = reminderToUpdate.Notes;
            item.Date = reminderToUpdate.Date;
            item.LastUpdateDate = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = reminderToUpdate.Id }, null);
        }

        #endregion

        #region | DeleteReminder - DELETE: /items/{id}

        [HttpDelete]
        [Route("items/{id:guid}")]
        public async Task<IActionResult> DeleteReminder(Guid id)
        {
            var item = await _context.ReminderItems.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.ReminderItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #endregion
    }
}
