using Abp.Modules;
using prod.Test.Base;

namespace prod.Tests
{
    [DependsOn(typeof(prodTestBaseModule))]
    public class prodTestModule : AbpModule
    {
       
    }
}
