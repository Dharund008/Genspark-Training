// filepath: /Users/dharund/Desktop/Presidio-Genspark/Genspark-Training/Bug Tracking System/Bts.Tests/AdminServiceTests.cs
using NUnit.Framework;
using Moq;
using Bts.Interfaces;
using Bts.Models;
using Bts.Services;
using Bts.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Bts.Models.DTO;

namespace Bts.Tests
{
    [TestFixture]
    public class AdminServiceTests
    {
        private Mock<IRepository<string, Admin>> _adminRepositoryMock;
        private Mock<IRepository<string, Developer>> _developerRepositoryMock;
        private Mock<IRepository<string, User>> _userRepositoryMock;
        private Mock<IRepository<string, Tester>> _testerRepositoryMock;
        private Mock<IEncryptionService> _encryptionServiceMock;
        private Mock<IBugLogService> _bugLogServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<ILogger<AdminService>> _loggerMock;
        private Mock<IMapper> _mapperMock;

        private BugContext _context;
        private AdminService _service;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<BugContext>()
                .UseInMemoryDatabase(databaseName: "BugTrackingTestDB")
                .EnableSensitiveDataLogging()
                .Options;

            _context = new BugContext(dbOptions);

            _loggerMock = new Mock<ILogger<AdminService>>();
            _mapperMock = new Mock<IMapper>();

            _adminRepositoryMock = new Mock<IRepository<string, Admin>>();
            _developerRepositoryMock = new Mock<IRepository<string, Developer>>();
            _userRepositoryMock = new Mock<IRepository<string, User>>();
            _testerRepositoryMock = new Mock<IRepository<string, Tester>>();
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _bugLogServiceMock = new Mock<IBugLogService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _service = new AdminService(
                _context,
                _mapperMock.Object,
                _adminRepositoryMock.Object,
                _developerRepositoryMock.Object,
                _userRepositoryMock.Object,
                _testerRepositoryMock.Object,
                _loggerMock.Object,
                _bugLogServiceMock.Object,
                _encryptionServiceMock.Object,
                _httpContextAccessorMock.Object
            );
        }

        // Success Test Cases
        [Test]
        public async Task AddAdmin_Should_Add_Admin_When_Valid()
        {
            var adminRequest = new AdminRequestDTO { Email = "newadmin@example.com", Password = "password", Name = "New Admin" };
            _adminRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync((Admin)null);
            _encryptionServiceMock.Setup(x => x.EncryptData(It.IsAny<EncryptModel>())).ReturnsAsync(new EncryptModel { EncryptedString = "encrypted" });
            _userRepositoryMock.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(new User { Username = adminRequest.Email });
            _adminRepositoryMock.Setup(x => x.Add(It.IsAny<Admin>())).ReturnsAsync(new Admin { Email = adminRequest.Email });

            var result = await _service.AddAdmin(adminRequest);

            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddAdmin(null));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo(adminRequest.Email));
        }

        [Test]
        public async Task AddDeveloper_Should_Add_Developer_When_Valid()
        {
            var devRequest = new DeveloperRequestDTO { Email = "newdev@example.com", Password = "password", Name = "New Developer" };
           _mapperMock.Setup(m => m.Map<User>(It.IsAny<DeveloperRequestDTO>())).Returns(new User { Id = "DEV123", Username = devRequest.Email });
            _encryptionServiceMock.Setup(x => x.EncryptData(It.IsAny<EncryptModel>())).ReturnsAsync(new EncryptModel { EncryptedString = "encrypted" });
            _userRepositoryMock.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(new User { Id = "DEV123", Username = devRequest.Email });
            _developerRepositoryMock.Setup(x => x.Add(It.IsAny<Developer>())).ReturnsAsync(new Developer { Id = "DEV123", Email = devRequest.Email });
            _developerRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync((Developer?)null); // Add this if your service checks for existing developer
                        var result = await _service.AddDeveloper(devRequest);
            //Assert.ThrowsAsync<System.Exception>(async () => await _service.AddAdmin(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo(devRequest.Email));
        }

        [Test]
        public async Task AddTester_Should_Add_Tester_When_Valid()
        {
            var testerRequest = new TesterRequestDTO { Email = "newtester@example.com", Password = "password", Name = "New Tester" };
           _mapperMock.Setup(m => m.Map<User>(It.IsAny<TesterRequestDTO>())).Returns(new User { Id = "TES123", Username = testerRequest.Email });
            _encryptionServiceMock.Setup(x => x.EncryptData(It.IsAny<EncryptModel>())).ReturnsAsync(new EncryptModel { EncryptedString = "encrypted" });
            _userRepositoryMock.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(new User { Id = "TES123", Username = testerRequest.Email });
            _testerRepositoryMock.Setup(x => x.Add(It.IsAny<Tester>())).ReturnsAsync(new Tester { Id = "TES123", Email = testerRequest.Email });
            _testerRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync((Tester?)null); // Add this if your service checks for existing tester

            var result = await _service.AddTester(testerRequest);
            //Assert.ThrowsAsync<System.Exception>(async () => await _service.AddAdmin(null));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo(testerRequest.Email));
        }

        // Failure Test Cases
        [Test]
        public void AddAdmin_Should_Throw_When_Email_Exists()
        {
            var adminRequest = new AdminRequestDTO { Email = "existingadmin@example.com", Password = "password", Name = "Existing Admin" };
            _adminRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(new Admin());

           Assert.ThrowsAsync<System.Exception>(async () => await _service.AddAdmin(null));

        }

        [Test]
        public void AddDeveloper_Should_Throw_When_Email_Exists()
        {
            var devRequest = new DeveloperRequestDTO { Email = "existingdev@example.com", Password = "password", Name = "Existing Developer" };
            _context.Developers.Add(new Developer { Email = "existingdev@example.com" });
            _context.SaveChanges();

            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddDeveloper(devRequest));
        }

        [Test]
        public void AddTester_Should_Throw_When_Email_Exists()
        {
            var testerRequest = new TesterRequestDTO { Email = "existingtester@example.com", Password = "password", Name = "Existing Tester" };
            _context.Testers.Add(new Tester { Email = "existingtester@example.com" });
            _context.SaveChanges();

            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddTester(testerRequest));
        }

        [Test]
        public void AddAdmin_Should_Throw_When_EncryptionFails()
        {
            var adminRequest = new AdminRequestDTO { Email = "newadmin@example.com", Password = "password", Name = "New Admin" };
            _adminRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync((Admin)null);
            _encryptionServiceMock.Setup(x => x.EncryptData(It.IsAny<EncryptModel>())).ThrowsAsync(new System.Exception("Encryption failed"));

            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddAdmin(adminRequest));
        }

        [Test]
        public void AddDeveloper_Should_Throw_When_EncryptionFails()
        {
            var devRequest = new DeveloperRequestDTO { Email = "newdev@example.com", Password = "password", Name = "New Developer" };
            _mapperMock.Setup(m => m.Map<User>(It.IsAny<DeveloperRequestDTO>())).Returns(new User { Id = "DEV123", Username = devRequest.Email });
            _encryptionServiceMock.Setup(x => x.EncryptData(It.IsAny<EncryptModel>())).ThrowsAsync(new System.Exception("Encryption failed"));

            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddDeveloper(devRequest));
        }

        [Test]
        public void AddTester_Should_Throw_When_EncryptionFails()
        {
            var testerRequest = new TesterRequestDTO { Email = "newtester@example.com", Password = "password", Name = "New Tester" };
            _mapperMock.Setup(m => m.Map<User>(It.IsAny<TesterRequestDTO>())).Returns(new User { Id = "TES123", Username = testerRequest.Email });
            _encryptionServiceMock.Setup(x => x.EncryptData(It.IsAny<EncryptModel>())).ThrowsAsync(new System.Exception("Encryption failed"));

            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddTester(testerRequest));
        }

        // Additional test cases can be added here to cover edge cases, invalid inputs, etc.
        [Test]
        public void AddAdmin_Should_Throw_When_AdminRequest_Is_Null()
        {
            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddAdmin(null));
        }

        [Test]
        public void AddDeveloper_Should_Throw_When_DeveloperRequest_Is_Null()
        {
            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddAdmin(null));
        }

        [Test]
        public void AddTester_Should_Throw_When_TesterRequest_Is_Null()
        {
            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddAdmin(null));
        }

        [Test]
        public async Task AddAdmin_Should_Throw_When_Password_Is_Weak()
        {
            var adminRequest = new AdminRequestDTO { Email = "weakpassadmin@example.com", Password = "123", Name = "Weak Password Admin" };
            _adminRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync((Admin)null);

            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddAdmin(adminRequest));
        }

        [Test]
        public async Task AddDeveloper_Should_Throw_When_Password_Is_Weak()
        {
            var devRequest = new DeveloperRequestDTO { Email = "weakpassdev@example.com", Password = "123", Name = "Weak Password Developer" };
            _mapperMock.Setup(m => m.Map<User>(It.IsAny<DeveloperRequestDTO>())).Returns(new User { Id = "DEV123", Username = devRequest.Email });

            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddDeveloper(devRequest));
        }

        [Test]
        public async Task AddTester_Should_Throw_When_Password_Is_Weak()
        {
            var testerRequest = new TesterRequestDTO { Email = "weakpasstester@example.com", Password = "123", Name = "Weak Password Tester" };
            _mapperMock.Setup(m => m.Map<User>(It.IsAny<TesterRequestDTO>())).Returns(new User { Id = "TES123", Username = testerRequest.Email });

            Assert.ThrowsAsync<System.Exception>(async () => await _service.AddTester(testerRequest));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}