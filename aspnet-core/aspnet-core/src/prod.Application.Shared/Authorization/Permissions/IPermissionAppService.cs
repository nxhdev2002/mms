using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Authorization.Permissions.Dto;

namespace prod.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
