using Microsoft.AspNetCore.Mvc;

[ApiController] // This attribute is used to indicate that this class is an API controller(automatic).
[Route("api/[controller]")] //controller replaces with the name class controller name without suffix(controller)..

public class PatientController : ControllerBase
{
    static List<Patient> pat = new List<Patient>
    {
        new Patient { Id = 1, Name = "John", Age = 25 , Gender = "Male", Email = "john@gmail.com", BloodGroup = "A+" },
        new Patient { Id = 2, Name = "Emma", Age = 28, Gender = "Female", Email = "emma@gmail.com", BloodGroup = "B+" },
        new Patient { Id = 3, Name = "Oliver", Age = 35, Gender = "Male", Email = "oliver@gmail.com", BloodGroup = "AB+" },
    };

    [HttpGet] //map this method to endpoint GET /api/Patient 
    public ActionResult GetPatient()
    {
        return Ok(pat);
    }

    [HttpPost]
    public ActionResult AddPatient([FromBody] Patient patient)
    {
        pat.Add(patient);
        return Created("", pat);// return the newly created patient
        //post 201 created
    }

    [HttpPut("{id}")] 
    public ActionResult UpdatePatient(int id, [FromBody] Patient patient)
    {
        var existingPatient = pat.FirstOrDefault(p => p.Id == id);
        if (existingPatient != null)
        {
            existingPatient.Name = patient.Name;
            existingPatient.Age = patient.Age;
            existingPatient.Gender = patient.Gender;
            existingPatient.Email = patient.Email;
            existingPatient.BloodGroup = patient.BloodGroup;
            return Ok(existingPatient); //statuscode 200
        }
        return NotFound(); //statuscode 404
    }

    [HttpPatch] //to just update age and bloodgroup 
    public ActionResult PartialUpdatePatient([FromQuery] int id, [FromBody] Patient patient)
    {
        var existingPatient = pat.FirstOrDefault(p => p.Id == id);
        if (existingPatient != null)
        {
            if (patient != null)
            {
                //existingPatient.Name = patient.Name ?? existingPatient.Name;
                existingPatient.Age = patient.Age;
                existingPatient.BloodGroup = patient.BloodGroup;
                return Ok(existingPatient);
            }
        }
        return NotFound();
    }

    [HttpDelete("{id}")]
    public ActionResult DeletePatient(int id)
    {
        var existingPatient = pat.FirstOrDefault(p => p.Id == id);
        if (existingPatient != null)
        {
            pat.Remove(existingPatient);
            return Ok();
        }
        return NotFound(); //statuscode 404
        return NoContent(); //statuscode 204
    }
}