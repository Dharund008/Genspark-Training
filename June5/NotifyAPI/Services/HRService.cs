using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotifyAPI.Contexts;
using NotifyAPI.Interfaces;
using NotifyAPI.Models;
using NotifyAPI.Models.DTO;
using NotifyAPI.Repositories;
using NotifyAPI.Misc;

namespace NotifyAPI.Services
{
    public class HRService : IHRService
    {
        private readonly ManagementContext _context;
        private readonly IMapper _mapper;
        private readonly IRepository<string, HRAdmin> _hrrepository;
        private readonly IRepository<string, Register> _registerRepository;
        private readonly IEncryptionService _encryptionService;

        public HRService(
            ManagementContext context,
            IMapper mapper,
            IRepository<string, HRAdmin> hrrepository,
            IRepository<string, Register> registerRepository,
            IEncryptionService encryptionService)
        {
            _context = context;
            _mapper = mapper;
            _hrrepository = hrrepository;
            _registerRepository = registerRepository;
            _encryptionService = encryptionService;
        }

        public async Task<HRAdmin> AddHRadmin(HRRequestDTO hr)
        {
            try
            {
                // var existingHR = await _hrrepository.Get(hr.Name);
                // if (existingHR != null)
                // {
                //     throw new Exception("HR Admin with this name already exists");
                // }

                var register = _mapper.Map<HRRequestDTO, Register>(hr);

                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = hr.Password,
                });
                register.Password = encryptedData.EncryptedData; // Store the encrypted password
                register.HashKey = encryptedData.HashKey; // Store the hash key for future encryption
                register.Role = "HR";
                register = await _registerRepository.Add(register);

                var newHR = new HRAdmin
                {
                    Name = hr.Name,
                    Email = hr.Email,
                    Department = hr.Department
                };
                newHR = await _hrrepository.Add(newHR);
                if (newHR == null)
                {
                    throw new Exception("Failed to add HR Admin");
                }
                return newHR;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HR services - Add: {ex.Message}");
                throw new Exception($"Failed to add HR {ex.Message}");
            }
        }

        public async Task<HRAdmin> GetHRAdmin(string email)
        {
            try
            {
                var newHR = await _context.HRAdmins.FirstOrDefaultAsync(h => h.Email == email);
                //in services, u will be using db table name (set in context) rather than class name 
                //but not same in repo ...
                return newHR;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HR Service - Get Email: {ex.Message}");
                throw new Exception("Failed to Get HR", ex);
            }
        }
    }
}
