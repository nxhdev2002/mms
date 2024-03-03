using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace prod.Web.Public.Views
{
    public abstract class prodRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected prodRazorPage()
        {
            LocalizationSourceName = prodConsts.LocalizationSourceName;
        }
    }
}
