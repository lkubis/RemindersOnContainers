using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience.Http;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.ViewModels.Account;

namespace WebMVC.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClient _httpClient;
        private readonly string _remoteIdentityBaseUrl;

        public IdentityService(
            IOptions<AppSettings> settings,
            IHttpContextAccessor httpContextAccessor,
            IHttpClient httpClient)
        {
            _settings = settings;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _remoteIdentityBaseUrl = $"{_settings.Value.IdentityUrl}/api/v1/auth";
        }

        public async Task<JwtTokenDTO> Token(string email, string password)
        {
            var tokenUri = API.Identity.Token(_remoteIdentityBaseUrl);

            var model = new LoginViewModel() { Email = email, Password = password };
            var response = await _httpClient.PostAsync(tokenUri, model);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<JwtTokenDTO>(responseString);
            return token;
        }
    }
}