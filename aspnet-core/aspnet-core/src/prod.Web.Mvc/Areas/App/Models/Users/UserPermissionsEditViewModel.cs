using Abp.AutoMapper;
using prod.Authorization.Users;
using prod.Authorization.Users.Dto;
using prod.Web.Areas.App.Models.Common;

namespace prod.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}