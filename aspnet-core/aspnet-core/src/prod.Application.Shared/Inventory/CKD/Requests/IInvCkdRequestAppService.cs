using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public interface IInvCkdRequestAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdRequestDto>> GetAllInvCkdRequestSearch(GetInvCkdRequestInput input);
        Task<PagedResultDto<InvCkdRequestContentByLotDto>> GetAllInvCkdByLot(GetInvCkdRequestDetailInput input);
        Task<PagedResultDto<InvCkdRequestContentByPxPDto>> GetAllInvCkdByPxP(GetInvCkdRequestDetailInput input);
        Task<PagedResultDto<GetInvCkdDeliveryScheGetByReqByMakeDto>> GetAllDeliveryScheduleByMake(GetCkdRequestDetailInput input);
        Task<PagedResultDto<GetInvCkdDeliveryScheGetByReqByCallDto>> GetAllDeliveryScheduleByCall(GetCkdRequestDetailInput input);

    }
}
