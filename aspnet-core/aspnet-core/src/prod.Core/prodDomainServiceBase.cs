using Abp.Domain.Services;

namespace prod
{
    public abstract class prodDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected prodDomainServiceBase()
        {
            LocalizationSourceName = prodConsts.LocalizationSourceName;
        }
    }
}
