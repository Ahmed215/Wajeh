using AutoMapper;
using Wajeeh.Authorization.Users;

namespace Wajeeh.Users.Dto
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<UserDto, User>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateUserDto, User>();
            CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());

            CreateMap<CreateUserByPhoneDto, User>();
            CreateMap<CreateUserByPhoneDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());
        }
    }
}
