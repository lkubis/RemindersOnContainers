using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Configuration.Sections
{
    public class IdentityOptions
    {
        public PasswordOptions Password { get; set; }
        public LockoutOptions Lockout { get; set; }
        public CookieOptions Cookie { get; set; }
        public UserOptions User { get; set; }
        public JwtSecurityTokenOptions JwtSecurityToken { get; set; }
    }

    public class PasswordOptions
    {
        public bool RequireDigit { get; set; }
        public int RequiredLength { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireLowercase { get; set; }
    }

    public class LockoutOptions
    {
        public int DefaultLockoutTimeSpan { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
    }

    public class CookieOptions
    {
        public int ExpireTimeSpan { get; set; }
        public string LoginPath { get; set; }
        public string LogoutPath { get; set; }
    }

    public class UserOptions
    {
        public string RequireUniqueEmail { get; set; }
    }

    public class JwtSecurityTokenOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public SymmetricSecurityKey SymetricSecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        public SigningCredentials SigningCredentials => new SigningCredentials(SymetricSecurityKey, SecurityAlgorithms.HmacSha256);
    }
}