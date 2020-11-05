using Abp.Authorization;
using Wajeeh.Authorization.Roles;
using Wajeeh.Authorization.Users;

namespace Wajeeh.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
