using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Wajeeh.Roles.Dto;
using Wajeeh.Users.Dto;

namespace Wajeeh.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);
        Task<UserDto> CreateByPhoneAsync(CreateUserByPhoneDto input);

        Task<bool> GetUserRoles(UserRoleDto input);
    }
}
