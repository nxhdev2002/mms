using Abp.Modules;
using Abp.Reflection.Extensions;

namespace prod
{
    public class prodCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(prodCoreSharedModule).GetAssembly());
        }
    }
}