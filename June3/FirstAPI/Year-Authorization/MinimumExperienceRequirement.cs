using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Authorization
{
    // Custom authorization attribute to enforce minimum experience requirement for doctors.
    public class MinimumExperienceRequirement : IAuthorizationRequirement
    {
        // the minimum years of experience required.
        public int Years { get; }

        // Constructor to initialize the minimum years of experience.
        public MinimumExperienceRequirement(int years)
        {
            Years = years;
        }
    }
}

/*
This class implements IAuthorizationRequirement, which is used by the ASP.NET Core authorization system
 to define custom requirements.
The constructor takes an integer parameter representing the minimum years of experience required 
 for a doctor.
The Years property is used to access this value later in the authorization handler.  
*/