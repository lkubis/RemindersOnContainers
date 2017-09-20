using System;
using Microsoft.IdentityModel.Tokens;

namespace Framework.Authentication.JwtBearer.Extensions
{
    public static class TokenValidationParametersExtensions
    {
        public static void AddOptions(this TokenValidationParameters parameters, 
            JwtSecurityTokenOptions options)
        {
            // The signing key must match!
            parameters.ValidateIssuerSigningKey = true;
            parameters.IssuerSigningKey = options.SymetricSecurityKey;
            // Validate the JWT Issuer (iss) claim
            parameters.ValidateIssuer = true;
            parameters.ValidIssuer = options.Issuer;
            // Validate the JWT Audience (aud) claim
            parameters.ValidateAudience = true;
            parameters.ValidAudience = options.Audience;
            // Validate the token expiry
            parameters.ValidateLifetime = true;
            // If you want to allow a certain amount of clock drift, set that here:
            parameters.ClockSkew = TimeSpan.Zero;
        }
    }
}