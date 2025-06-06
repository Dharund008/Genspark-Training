using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using OAuth.Interfaces;

namespace OAuth.Services
{
    public class AuthenticationService : OAuth.Interfaces.IAuthenticationService
    {
        private readonly TokenGenerator _tokenGenerator;

        public AuthenticationService(TokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> HandleGoogleLogin(HttpContext httpContext)
        {
            var result = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                throw new Exception("Authentication failed.");
            }

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email claim not found.");
            }

            return _tokenGenerator.GenerateJwtToken(email);
        }
    }
}