using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Repositories;
using FirstAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    public async Task AddDoctorTest()
    {
        //Assert.Pass();

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
        var result = await _doctorRepository.Add(doctor);
        //3.assert
        Assert.That(result, Is.Not.Null, "Doctor IS not addeed");
        Assert.That(result.Id, Is.EqualTo(1)); //2 : error;
    }

    [Test]
    public async Task GetDoctorByIdTest()
    {
        //Arrange
        var doc = new Doctor
        {
            Name = "dr.A",
            YearsOfExperience = 5,
            Email = "a@gmail.com"
        };
        _context.Add(doc);
        await _context.SaveChangesAsync();

        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        //2.action
        var result = await _doctorRepository.Get(doc.Id);
        //3.assert
        //Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(doc.Id)); 
    }

    [Test]
    public async Task GetAllDoctorTest()
    {
        //1.Arrange
        _context.Doctors.AddRange(
        new Doctor { Name = "dr. A", YearsOfExperience = 1, Email = "doca@gmail.com" },
        new Doctor { Name = "dr. B", YearsOfExperience = 10, Email = "docb@gmail.com" }
        );

        await _context.SaveChangesAsync();

        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        //2.action
        var result = await _doctorRepository.GetAll();
        //3.assert
        //Assert.IsNotNull(result);
        Assert.That(result.Count(), Is.EqualTo(2)); 
    }

    [Test]
    public async Task UpdateDoctorTest()
    {
        //1.Arrange
        var doc = new Doctor
        {
            Name = "dr.A",
            YearsOfExperience = 5,
            Email = "a@gmail.com"
        };
        _context.Add(doc);
        await _context.SaveChangesAsync();

        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);

        var updDoc = new Doctor
        {
            Id = doc.Id,
            Name = "dr.A updated",
            YearsOfExperience = 6,
            Email = "Aupdated@gmail.com"
        };
        //2.action
        var result = await _doctorRepository.Update(doc.Id, updDoc);

        //3.assert
        //Assert.IsNotNull(result);
        Assert.That(result.Name, Is.EqualTo("dr.A updated"));
        Assert.That(result.YearsOfExperience, Is.EqualTo(6));
    }

    [Test]
    public async Task DeleteDoctorTest()
    {
        //1.Arrange
        var doc = new Doctor
        {
            Name = "dr.A",
            YearsOfExperience = 5,
            Email = "a@gmail.com"
        };
        _context.Add(doc);
        await _context.SaveChangesAsync();

        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);

        //2.action
        var result = await _doctorRepository.Delete(doc.Id);

        //3.assert
        //Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(doc.Id));
        Assert.That(_context.Doctors.Any(d => d.Id == doc.Id), Is.False, "Doctor should be deleted from the database");
    }


    [TestCase(222)]
    public void GetDoctorExceptionTest(int id)
    {
        // //1.Arrange
        // var email = "test@gmail.com";
        // var password = System.Text.Encoding.UTF8.GetBytes("test123");
        // var key = Guid.NewGuid().ToByteArray();
        // var user = new User
        // {
        //     Username = email,
        //     Password = password,
        //     HashKey = key,
        //     Role = "Doctor"
        // };
        // _context.Add(user);
        // await _context.SaveChangesAsync();
        // var doctor = new Doctor
        // {
        //     Name = "tester",
        //     YearsOfExperience = 2,
        //     Email = email
        // };
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        //2.action

        //exceptions
        var ex = Assert.ThrowsAsync<Exception>(async () => await _doctorRepository.Get(id));

        //3.assert
        Assert.That(ex.Message, Is.EqualTo($"Doctor with id: {id} not found"));
    }

    [Test]
    public void GetAllDoctorsException()
    {
        //1.Arrange
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        //2.action

        //exceptions
        var ex = Assert.ThrowsAsync<Exception>(async () => await _doctorRepository.GetAll());

        //3.assert
        Assert.That(ex.Message, Is.EqualTo("No Doctor in the database"));
    }

    [Test]
    public void DeleteDoctorExceptionTest()
    {
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);

        //2.action
        var ex = Assert.ThrowsAsync<Exception>(async () => await _doctorRepository.Delete(222));

        //3.assert
        Assert.That(ex.Message, Is.EqualTo("Doctor with id: 222 not found"));
    }



    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}
