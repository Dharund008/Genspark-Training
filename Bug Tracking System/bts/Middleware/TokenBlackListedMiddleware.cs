using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Bts.Interfaces;
using Bts.Models;

namespace Bts.MiddleWare
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IBlacklistedTokenRepository blacklistRepo)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length);

                bool isBlacklisted = await blacklistRepo.IsTokenBlacklistedAsync(token);
                if (isBlacklisted)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized: Token is blacklisted.");
                    return;
                }
            }

            await _next(context);
        }
    }
}