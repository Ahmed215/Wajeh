using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Wajeeh.Authorization;
using Wajeeh.ChatRooms;

namespace Wajeeh
{
    [DependsOn(
        typeof(WajeehCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class WajeehApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<WajeehAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(WajeehApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);
            Configuration.Notifications.Notifiers.Add<MessageNotifier>();
            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
