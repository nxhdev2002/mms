using Abp.AspNetCore.Mvc.Authorization;
using prod.Authorization;
using prod.Storage;
using Abp.BackgroundJobs;
using Abp.Authorization;

namespace prod.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}