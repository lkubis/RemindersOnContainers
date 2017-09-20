using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IIdentityService
    {
        Task<JwtTokenDTO> Token(string email, string password);
    }
}