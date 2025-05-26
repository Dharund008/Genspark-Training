using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

public class DoctorController : ControllerBase
{
    static List<Doctor> doctors = new List<Doctor>
    {
        new Doctor { Id = 1, Name = "Dr. Smith" },
        new Doctor { Id = 2, Name = "Dr. Johnson" },
    };
    //Declared static so the list isn't re-initialized with every HTTP request.
    //The objects gets redeclared after every request. So , we need to declare it static.
    //This is the best practice to declare static variables in the controller.
    [HttpGet]
    public IActionResult GetDoctors()
    {
        return Ok(doctors);
    }

    [HttpPost] //POST request
    public IActionResult CreateDoctor([FromBody] Doctor doctor)
    {
        doctors.Add(doctor);
        return Created(" ", doctors);//Created is used to return the created resource
    }

    [HttpPut] //PUT request
    //also, [HttpPut ("{id}")] is used to update the specific doctor.
    public IActionResult UpdateDoctor(int id, [FromBody] Doctor doctor) //fromBody is used to get the data from the body of the request(json)
    // (which tells the framework to get the data from the body of the request)
    {
        var existingDoctor = doctors.FirstOrDefault(d => d.Id == id);
        if (existingDoctor != null)
        {
            existingDoctor.Name = doctor.Name;
            return Ok(existingDoctor);
        }
        return NotFound();
    }

    [HttpPatch("{id}")] //PATCH request
                        //It is used to update specific properties of the object.
    public IActionResult PatchDoctor(int id, [FromBody] Doctor doctor)
    {
        var existingDoctor = doctors.FirstOrDefault(d => d.Id == id);

        if (existingDoctor == null)
        {
            return NotFound();
        }
        if (!string.IsNullOrEmpty(doctor.Name))
        {
            existingDoctor.Name = doctor.Name;
        }

        return Ok(existingDoctor);
    }


    [HttpDelete] //DELETE request
                 // This is used to delete the specific doctor.

    public IActionResult DeleteDoctor(int id)
    {
        var existingDoctor = doctors.FirstOrDefault(d => d.Id == id);
        if (existingDoctor != null)
        {
            doctors.Remove(existingDoctor);
        }
        return NoContent();
    }
    
}

