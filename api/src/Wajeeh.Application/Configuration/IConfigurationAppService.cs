using System.Threading.Tasks;
using Wajeeh.Configuration.Dto;

namespace Wajeeh.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
