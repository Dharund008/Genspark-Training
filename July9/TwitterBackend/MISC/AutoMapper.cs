using AutoMapper;
using Twitter.Models;
using Twitter.DTOs;

namespace Twitter.MISC
{
    public class AutoMapperTwitter : Profile //base class
    //configuration container for AutoMapper.
    //Profile is like a blueprint that AutoMapper reads during startup.
    {
        public AutoMapperTwitter()
        {
            //entity to dto
            CreateMap<User, ResponseUserClass>(); //map datas from User to ResponseUserClass
            // CreateMap<Post, PostDTO>();
            // CreateMap<UserProfile, ProfileDTO>();
            // CreateMap<Like, LikeDTO>();
            // CreateMap<Hashtag, HashtagDTO>();
            // CreateMap<Follow, FollowDTO>();

            //dto to entity
            CreateMap<CreateUserDTO, User>();//map created data to user
            CreateMap<UpdateUserDTO, User>(); //map updated data to user
        }
    }
}