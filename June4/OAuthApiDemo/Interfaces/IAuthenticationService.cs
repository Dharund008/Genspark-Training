using System;

namespace OAuth.Interfaces
{

    public interface IAuthenticationService
    {
        public Task<string> HandleGoogleLogin(HttpContext httpContext);
    }
}