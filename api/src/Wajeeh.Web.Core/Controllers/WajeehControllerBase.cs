using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Wajeeh.Controllers
{
    public abstract class WajeehControllerBase: AbpController
    {
        protected WajeehControllerBase()
        {
            LocalizationSourceName = WajeehConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
