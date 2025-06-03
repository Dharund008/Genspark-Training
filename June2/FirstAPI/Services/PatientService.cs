using System.Threading.Tasks;
using AutoMapper;
using FirstAPI.Interfaces;
using FirstAPI.Misc;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;


namespace FirstAPI.Services
{
    public class PatientService : IPatientService
    {
        PatientMapper patientmapper;
        private readonly IRepository<int, Patient> _patientRepository;
        private readonly IRepository<string, User> _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;

        public PatientService(IRepository<int, Patient> patientrepository,
                            IRepository<string, User> userrepository,
                            IEncryptionService encryptionService,
                            IMapper mapper)
        {
            patientmapper = new PatientMapper();
            _patientRepository = patientrepository;
            _userRepository = userrepository;
            _encryptionService = encryptionService;
            _mapper = mapper;
        }

        public async Task<Patient> AddPatient(PatientAddRequestDTO patient)
        {
            try
            {
                var user = _mapper.Map<PatientAddRequestDTO, User>(patient); //map the request to user 
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = patient.Password
                });
                user.Password = encryptedData.EncryptedData; //storing the encrypted password in user. 
                user.HashKey = encryptedData.HashKey;   //storing the hash key in user.
                user.Role = "Patient";
                user = await _userRepository.Add(user); // add the user to the database
                var newPatient = patientmapper.MapPatientAddRequestDoctor(patient); // mapping the patientAddRequestDto to Patient
                newPatient = await _patientRepository.Add(newPatient); // adding the patient to the database.
                if (newPatient == null)
                {
                    throw new Exception("Failed to add patient");
                }
                return newPatient;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}