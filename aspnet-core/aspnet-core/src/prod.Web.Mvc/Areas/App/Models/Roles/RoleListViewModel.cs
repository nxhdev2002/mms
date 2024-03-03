using System.Collections.Generic;
using Abp.Application.Services.Dto;
using prod.Authorization.Permissions.Dto;
using prod.Web.Areas.App.Models.Common;

namespace prod.Web.Areas.App.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}