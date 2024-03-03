using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace prod.Startup
{
    [DependsOn(typeof(prodCoreModule))]
    public class prodGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(prodGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}