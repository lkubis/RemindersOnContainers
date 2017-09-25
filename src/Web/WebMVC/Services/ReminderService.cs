using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience.Http;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.ViewModels.Reminder;

namespace WebMVC.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClient _httpClient;
        private readonly string _remoteReminderBaseUrl;

        public ReminderService(
            IOptions<AppSettings> settings,
            IHttpContextAccessor httpContextAccessor,
            IHttpClient httpClient)
        {
            _settings = settings;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _remoteReminderBaseUrl = $"{_settings.Value.ReminderUrl}/api/v1/reminders";
        }

        public async Task<PaginatedItemsViewModel<ReminderViewModel>> Items(int pageSize, int pageIndex)
        {
            var itemsUri = API.Reminders.Items(_remoteReminderBaseUrl, pageSize, pageIndex);
            var accessToken = _httpContextAccessor.HttpContext.User.FindFirst("access_token")?.Value;
            var content = await _httpClient.GetStringAsync(itemsUri, authorizationToken: accessToken);
            return JsonConvert.DeserializeObject<PaginatedItemsViewModel<ReminderViewModel>>(content);
        }
    }
}