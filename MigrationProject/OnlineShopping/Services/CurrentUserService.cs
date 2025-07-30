
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Online.Interfaces;

namespace Online.Services;
public class CurrentUserService : ICurrentUserService
{
    public int Id { get; }

    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        var user = contextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            var id = user.FindFirst("MyApp_Id")?.Value;
            if (!string.IsNullOrEmpty(id))
            {
                var res = int.Parse(id);
                Id = res;
            }
        }
    }
}