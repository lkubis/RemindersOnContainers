using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reminder.API.Models.Entities
{
    [Table("Reminders")]
    public class ReminderItem
    {
        #region | Properties

        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(64)]
        public string Title { get; set; }

        public DateTimeOffset Date { get; set; }

        public string Notes { get; set; }

        public DateTimeOffset? CompletionDate { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset? LastUpdateDate { get; set; }

        #endregion
    }
}