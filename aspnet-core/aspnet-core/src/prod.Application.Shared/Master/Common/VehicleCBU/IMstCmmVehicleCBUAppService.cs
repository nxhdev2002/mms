using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Common.VehicleCBU.Dto;
using System.Threading.Tasks;

namespace prod.Master.Common.VehicleCBU
{
    public interface IMstCmmVehicleCBUAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmVehicleCBUDto>> GetVehicleCBUSearch(GetMstCmmVehicleCBUInput input);

        Task<PagedResultDto<MstCmmVehicleCBUColorDto>> GetVehicleCBUColorById(GetMstCmmVehicleCBUColorInput input);

        Task UpdateCreateMaterial(UpdateCmmVehicleCBUCreateMaterial input);

    }
}
