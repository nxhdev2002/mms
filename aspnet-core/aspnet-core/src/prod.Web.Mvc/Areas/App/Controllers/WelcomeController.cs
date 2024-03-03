using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using prod.Web.Controllers;

namespace prod.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class WelcomeController : prodControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}