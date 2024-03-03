using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using prod.Web.Areas.App.Models.Layout;
using prod.Web.Session;
using prod.Web.Views;

namespace prod.Web.Areas.App.Views.Shared.Themes.Theme13.Components.AppTheme13Brand
{
    public class AppTheme13BrandViewComponent : prodViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme13BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
