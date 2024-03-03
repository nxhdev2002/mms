using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Common.Dto;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    public interface IMstCmmStorageLocationCategoryAppService : IApplicationService
    {
        Task<PagedResultDto<MstCmmStorageLocationCategoryDto>> GetAll(GetMstCmmStorageLocationCategoryInput input);

    }

}

