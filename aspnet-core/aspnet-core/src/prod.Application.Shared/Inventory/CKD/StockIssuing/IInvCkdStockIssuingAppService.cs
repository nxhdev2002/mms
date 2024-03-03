using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public interface IInvCkdStockIssuingAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdStockIssuingDto>> GetAll(GetInvCkdStockIssuingInput input);
        Task<PagedResultDto<InvCkdStockIssuingTranslocDto>> GetDataStockIssuingView(GetInvCkdStockIssuingViewInput input);
        Task<PagedResultDto<InvCkdStockIssuingValidateDto>> GetValidateStockIssuing(GetInvCkdStockIssuingValidateInput input);
    }

}


