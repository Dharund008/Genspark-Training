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

public class PatientRepoTest
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
    public async Task AddPatientTest()
    {
        //Arrange
        var email = "test@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Patient"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();

        var patient = new Patient
        {
            Name = "Test",
            Age = 32,
            Email = email,
            Phone = "1234567890"
        };

        IRepository<int, Patient> patientRepository = new PatinetRepository(_context);

        //Act
        var result = await patientRepository.Add(patient);

        //Assert
        Assert.That(result, Is.Not.Null, "Patient is not added");
        Assert.That(result.Id, Is.EqualTo(1), "Patient ID is not as expected");
    }
}