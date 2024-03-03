using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using prod.Configure;
using prod.Startup;
using prod.Test.Base;

namespace prod.GraphQL.Tests
{
    [DependsOn(
        typeof(prodGraphQLModule),
        typeof(prodTestBaseModule))]
    public class prodGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(prodGraphQLTestModule).GetAssembly());
        }
    }
}