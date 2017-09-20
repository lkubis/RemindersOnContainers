using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Framework.Authentication.JwtBearer
{
    public class JwtTicketDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _algorithm;
        private readonly TokenValidationParameters _validationParameters;

        public JwtTicketDataFormat(string algorithm, TokenValidationParameters validationParameters)
        {
            this._algorithm = algorithm;
            this._validationParameters = validationParameters;
        }

        public AuthenticationTicket Unprotect(string protectedText)
            => Unprotect(protectedText, null);

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;
            SecurityToken validToken = null;

            try
            {
                principal = handler.ValidateToken(protectedText, this._validationParameters, out validToken);

                var validJwt = validToken as JwtSecurityToken;

                if (validJwt == null)
                    throw new ArgumentException("Invalid JWT");

                if (!validJwt.Header.Alg.Equals(_algorithm, StringComparison.Ordinal))
                    throw new ArgumentException($"Algorithm must be '{_algorithm}'");
            }
            catch (SecurityTokenValidationException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }

            // VALIDATION PASSED
            return new AuthenticationTicket(principal, new AuthenticationProperties(), "Cookies");
        }

        public string Protect(AuthenticationTicket data)
        {
            return Protect(data, null);
        }

        public string Protect(AuthenticationTicket data, string purpose)
        {
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _validationParameters.ValidIssuer,
                audience: _validationParameters.ValidAudience,
                claims: data.Principal.Claims,
                expires: DateTime.Parse(data.Properties.Items["token_expires"]),
                signingCredentials: new SigningCredentials(_validationParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}