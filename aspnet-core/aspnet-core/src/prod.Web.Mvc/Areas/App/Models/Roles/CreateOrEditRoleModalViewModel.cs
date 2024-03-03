using Abp.AutoMapper;
using prod.Authorization.Roles.Dto;
using prod.Web.Areas.App.Models.Common;

namespace prod.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}