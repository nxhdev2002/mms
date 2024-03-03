using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using prod.Inventory.SPP.InvoiceDetails.Dto;

namespace prod.Inventory.SPP.InvoiceDetails
{
    public interface IInvSppInvoiceDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<InvSppInvoiceDetailsDto>> GetAll(GetInvSppInvoiceDetailsInput input);
    }
}
