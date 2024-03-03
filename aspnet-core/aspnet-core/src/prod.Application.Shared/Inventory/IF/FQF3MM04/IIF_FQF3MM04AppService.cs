using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.IF.FQF3MM04.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.IF
{
    public interface IIF_FQF3MM04AppService : IApplicationService
    {
        Task<PagedResultDto<IF_FQF3MM04Dto>> GetAll(GetIF_FQF3MM04Input input);
    }
}