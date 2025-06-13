using AutoMapper;
using Bts.Models;
using Bts.Models.DTO;

namespace Bts.MISC;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AdminRequestDTO, User>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<User, AdminRequestDTO>()
                .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Username));

        CreateMap<AdminRequestDTO, Admin>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<DeveloperRequestDTO, User>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<User, DeveloperRequestDTO>()
                .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Username));

        CreateMap<DeveloperRequestDTO, Developer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<TesterRequestDTO, User>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<User, TesterRequestDTO>()
                .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Username));

        CreateMap<TesterRequestDTO, Tester>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
            
        // CreateMap<BugSubmissionDTO, Bug>()
        //     .ForMember(dest => dest.Id, opt => opt.Ignore())
        //     .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BugStatus.New));

    }
}