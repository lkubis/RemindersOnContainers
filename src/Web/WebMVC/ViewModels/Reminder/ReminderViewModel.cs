using System;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.ViewModels.Reminder
{
    public class ReminderViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset Date { get; set; }
        public string Notes { get; set; }

        [Display(Name = "Completion Date")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset? CompletionDate { get; set; }

        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset? LastUpdateDate { get; set; }
    }
}