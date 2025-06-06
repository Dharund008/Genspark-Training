using System.Security.Claims;
using FirstAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Authorization
{
    public class ExperienceHandler : AuthorizationHandler<MinimumExperienceRequirement>
    {
        private readonly IDoctorService _doctorservice;

        public ExperienceHandler(IDoctorService doctorservice)
        {
            _doctorservice = doctorservice;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumExperienceRequirement requirement)
        {
            // Check if the user is authenticated
            if (!context.User.Identity.IsAuthenticated)
            {
                return;
            }   

            // Get the username from the claims
            var username = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(username))
            {
                return; // No username found in claims
            }

            var doctor = await _doctorservice.GetDoctByName(username);

            // Checking the doctor has enough experience
            if (doctor != null && doctor.YearsOfExperience > requirement.Years)
            {
                context.Succeed(requirement);
            }

            await Task.CompletedTask; //cannot return a value in an async Task method.
        }
    }
}