using Abp.Modules;
using Abp.Reflection.Extensions;

namespace prod
{
    [DependsOn(typeof(prodCoreSharedModule))]
    public class prodApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(prodApplicationSharedModule).GetAssembly());
        }
    }
}