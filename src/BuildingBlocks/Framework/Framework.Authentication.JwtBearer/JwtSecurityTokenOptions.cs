using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Framework.Authentication.JwtBearer
{
    public class JwtSecurityTokenOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        
        // In minutes
        public int Expires { get; set; }

        public SymmetricSecurityKey SymetricSecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        public SigningCredentials SigningCredentials => new SigningCredentials(SymetricSecurityKey, SecurityAlgorithms.HmacSha256);
    }
}