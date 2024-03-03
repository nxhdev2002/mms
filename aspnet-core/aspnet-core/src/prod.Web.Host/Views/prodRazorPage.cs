using Abp.AspNetCore.Mvc.Views;

namespace prod.Web.Views
{
    public abstract class prodRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected prodRazorPage()
        {
            LocalizationSourceName = prodConsts.LocalizationSourceName;
        }
    }
}
