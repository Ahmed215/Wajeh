using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Wajeeh.Configuration.Dto;

namespace Wajeeh.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : WajeehAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
