using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Repositories;
using FirstAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Test;

public class Tests
{
    private ClinicContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
                        .UseInMemoryDatabase("TestDb")
                        .Options;
        _context = new ClinicContext(options);
    }

    [Test]
    // public async Task AddDoctorTest()
    // {
    //     //Assert.Pass();

    //     //1.Arrange
    //     var email = "test@gmail.com";
    //     var password = System.Text.Encoding.UTF8.GetBytes("test123");
    //     var key = Guid.NewGuid().ToByteArray();
    //     var user = new User
    //     {
    //         Username = email,
    //         Password = password,
    //         HashKey = key,
    //         Role = "Doctor"
    //     };
    //     _context.Add(user);
    //     await _context.SaveChangesAsync();
    //     var doctor = new Doctor
    //     {
    //         Name = "tester",
    //         YearsOfExperience = 2,
    //         Email = email
    //     };
    //     IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
    //     //2.action
    //     var result = await _doctorRepository.Add(doctor);
    //     //3.assert
    //     Assert.That(result, Is.Not.Null, "Doctor IS not addeed");
    //     Assert.That(result.Id, Is.EqualTo(1)); //2 : error;
    // }

    [TestCase(3)]
    public async Task GetDoctorExceptionTest(int id)
    {
        //1.Arrange
        var email = "test@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var doctor = new Doctor
        {
            Name = "tester",
            YearsOfExperience = 2,
            Email = email
        };
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        //2.action
        await _doctorRepository.Add(doctor);

        //exceptions
        var ex = await Assert.ThrowsAsync<Exception>(async () => await _doctorRepository.Get(id));

        //3.assert
        Assert.That(ex.Message, Is.EqualTo($"Doctor with id: {id} not found"));
    }


    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}
