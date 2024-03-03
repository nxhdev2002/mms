using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using prod.Authorization;

namespace prod
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(prodApplicationSharedModule),
        typeof(prodCoreModule)
        )]
    public class prodApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(prodApplicationModule).GetAssembly());
        }
    }
}