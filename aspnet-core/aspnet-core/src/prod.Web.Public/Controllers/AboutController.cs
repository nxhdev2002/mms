using Microsoft.AspNetCore.Mvc;
using prod.Web.Controllers;

namespace prod.Web.Public.Controllers
{
    public class AboutController : prodControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}