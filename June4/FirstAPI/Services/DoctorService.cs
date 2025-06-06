using System.Threading.Tasks;
using AutoMapper;
using FirstAPI.Interfaces;
using FirstAPI.Misc;
using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.EntityFrameworkCore;


namespace FirstAPI.Services
{
    public class DoctorService : IDoctorService
    {
        DoctorMapper doctorMapper ;
        SpecialityMapper specialityMapper;
        private readonly ClinicContext _clinicContext;
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, Speciality> _specialityRepository;
        private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;
        private readonly IRepository<string, User> _userRepository;
        private readonly IOtherContextFunctionities _otherContextFunctionities;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;

        public DoctorService(IRepository<int, Doctor> doctorRepository,
                            IRepository<int, Speciality> specialityRepository,
                            IRepository<int, DoctorSpeciality> doctorSpecialityRepository,
                            ClinicContext clinicContext,
                            IRepository<string, User> userRepository,
                            IOtherContextFunctionities otherContextFunctionities,
                            IEncryptionService encryptionService,
                            IMapper mapper)
        {
            doctorMapper = new DoctorMapper();
            specialityMapper = new();
            _clinicContext = clinicContext;
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _doctorSpecialityRepository = doctorSpecialityRepository;
            _userRepository = userRepository;
            _otherContextFunctionities = otherContextFunctionities;
            _encryptionService = encryptionService;
            _mapper = mapper;

        }

        public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctor)
        {
            try
            {
                var user = _mapper.Map<DoctorAddRequestDto, User>(doctor);
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data= doctor.Password
                });
                user.Password = encryptedData.EncryptedData;
                user.HashKey = encryptedData.HashKey;
                user.Role = "Doctor";
                user = await _userRepository.Add(user);
                var newDoctor = doctorMapper.MapDoctorAddRequestDoctor(doctor);
                newDoctor = await _doctorRepository.Add(newDoctor);
                if (newDoctor == null)
                    throw new Exception("Could not add doctor");
                if (doctor.Specialities.Count() > 0)
                {
                    int[] specialities = await MapAndAddSpeciality(doctor);
                    for (int i = 0; i < specialities.Length; i++)
                    {
                        var doctorSpeciality = specialityMapper.MapDoctorSpecility(newDoctor.Id, specialities[i]);
                        doctorSpeciality = await _doctorSpecialityRepository.Add(doctorSpeciality);
                    }
                }
                return newDoctor;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        private async Task<int[]> MapAndAddSpeciality(DoctorAddRequestDto doctor)
        {
            int[] specialityIds = new int[doctor.Specialities.Count()];
            IEnumerable<Speciality> existingSpecialities = null;
            try
            {
                existingSpecialities = await _specialityRepository.GetAll();
            }
            catch (Exception e)
            {

            }
            int count = 0;
            foreach (var item in doctor.Specialities)
            {
                Speciality speciality = null;
                if (existingSpecialities != null)
                    speciality = existingSpecialities.FirstOrDefault(s => s.Name.ToLower() == item.Name.ToLower());
                if (speciality == null)
                {
                    speciality = specialityMapper.MapSpecialityAddRequestDoctor(item);
                    speciality = await _specialityRepository.Add(speciality);
                }
                specialityIds[count] = speciality.Id;
                count++;
            }
            return specialityIds;
        }

        public async Task<Doctor> GetDoctByName(string name)
        {
            var doc = await _clinicContext.Doctors.FirstOrDefaultAsync(d => d.Name == name);
            return doc;
        }
        public async Task<Doctor> GetDoctByEmail(string name)
        {
            var doc = await _clinicContext.Doctors.FirstOrDefaultAsync(d => d.Email == name);
            return doc;
        }

        public async Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string speciality)
        {
            var result = await _otherContextFunctionities.GetDoctorsBySpeciality(speciality);
            return result;
        }
    }
}