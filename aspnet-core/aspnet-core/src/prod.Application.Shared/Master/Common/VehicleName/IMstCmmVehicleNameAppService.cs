using Abp.Application.Services.Dto;
using Abp.Application.Services;
using prod.Master.Common.Dto;
using System.Threading.Tasks;

namespace prod.Master.Common
{

    public interface IMstCmmVehicleNameAppService : IApplicationService
    {
        Task<PagedResultDto<MstCmmVehicleNameDto>> GetAll(GetMstCmmVehicleNameInput input);

    }

}
