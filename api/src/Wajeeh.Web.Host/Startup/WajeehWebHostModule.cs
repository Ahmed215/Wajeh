using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Wajeeh.Configuration;
using Abp.AspNetCore.SignalR;

namespace Wajeeh.Web.Host.Startup
{
    [DependsOn(
       typeof(WajeehWebCoreModule))]
    [DependsOn(typeof(AbpAspNetCoreSignalRModule))]
    public class WajeehWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public WajeehWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WajeehWebHostModule).GetAssembly());
        }
    }
}
