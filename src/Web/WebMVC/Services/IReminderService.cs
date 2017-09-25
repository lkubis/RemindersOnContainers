using System.Collections.Generic;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.ViewModels.Reminder;

namespace WebMVC.Services
{
    public interface IReminderService
    {
        Task<PaginatedItemsViewModel<ReminderViewModel>> Items(int pageSize = 10, int pageIndex = 0);
    }
}