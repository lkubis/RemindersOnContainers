using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Configuration.Sections
{
    public class IdentityOptions
    {
        public PasswordOptions Password { get; set; }
        public LockoutOptions Lockout { get; set; }
        public UserOptions User { get; set; }
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

    public class UserOptions
    {
        public string RequireUniqueEmail { get; set; }
    }
}