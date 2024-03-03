using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.IF.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.IF
{
    public interface IIF_FQF3MM_LV2AppService : IApplicationService
    {

        Task<PagedResultDto<IF_FQF3MM_LV2Dto>> GetAll(GetIF_FQF3MM_LV2Input input);

        Task<PagedResultDto<IF_FQF3MM_LV2Dto>> GetFQF3MMLV2byId(int id);

    }

}