using Abp.AspNetCore.Mvc.ViewComponents;

namespace prod.Web.Public.Views
{
    public abstract class prodViewComponent : AbpViewComponent
    {
        protected prodViewComponent()
        {
            LocalizationSourceName = prodConsts.LocalizationSourceName;
        }
    }
}