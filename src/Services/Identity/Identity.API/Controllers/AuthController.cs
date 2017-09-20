using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Framework.Authentication.JwtBearer;
using Identity.API.Configuration;
using Identity.API.Models;
using Identity.API.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Identity.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        #region | Fields

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ServiceConfiguration _configuration;
        private readonly JwtSecurityTokenOptions _jwtOptions;
        
        #endregion

        #region | Constructors

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IOptions<ServiceConfiguration> configurationOptions,
            IOptions<JwtSecurityTokenOptions> jwtOptions)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _configuration = configurationOptions.Value;
            _jwtOptions = jwtOptions.Value;
        }

        #endregion

        #region | Public Methods

        #region | POST: /register

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return Ok(result);

            foreach (var error in result.Errors)
                ModelState.AddModelError("error", error.Description);
            return BadRequest(result.Errors);
        }

        #endregion

        #region | POST: /token

        [HttpPost("CreateToken")]
        [Route("token")]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return Unauthorized();

                if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Failed)
                    return Unauthorized();

                var userClaims = await _userManager.GetClaimsAsync(user);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                }.Union(userClaims);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtOptions.Expires),
                    signingCredentials: _jwtOptions.SigningCredentials
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    expiration = jwtSecurityToken.ValidTo
                });

            }
            catch (Exception exception)
            {
                // TODO: Log exception
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error while creating token.");
            }
        }

        #endregion

        #endregion
    }
}