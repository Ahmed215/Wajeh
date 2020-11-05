using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Wajeeh.Authorization
{
    public class WajeehAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Drivers, L("Drivers"));
            context.CreatePermission(PermissionNames.Pages_Clients, L("Clients"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, WajeehConsts.LocalizationSourceName);
        }
    }
}
