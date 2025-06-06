using AutoMapper;
using NotifyAPI.Models;
using NotifyAPI.Models.DTO;


namespace NotifyAPI.Misc
{
    public class AutoMapperNotify : Profile
    {
        public AutoMapperNotify()
        {
            CreateMap<HRRequestDTO, Register>()
                .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<Register, HRRequestDTO>()
                .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Username));

            //User Mapping
            CreateMap<UserRequestDTO, Register>()
                .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<Register, UserRequestDTO>()
                .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Username));

        }
    }
}