using Abp.AspNetCore.Mvc.ViewComponents;

namespace prod.Web.Views
{
    public abstract class prodViewComponent : AbpViewComponent
    {
        protected prodViewComponent()
        {
            LocalizationSourceName = prodConsts.LocalizationSourceName;
        }
    }
}