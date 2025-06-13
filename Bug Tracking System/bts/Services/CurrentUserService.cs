using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Bts.Interfaces;
public class CurrentUserService : ICurrentUserService
    {
        public string Id { get; } = string.Empty;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            var user = contextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var id = user.FindFirst("MyApp_Id")?.Value;
                if (!string.IsNullOrEmpty(id))
                {
                    Id = id;
                }
            }
        }
    }