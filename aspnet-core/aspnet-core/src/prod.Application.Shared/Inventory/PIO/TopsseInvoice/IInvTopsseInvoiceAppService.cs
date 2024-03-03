using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.PIO.TopsseInvoice.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.TopsseInvoice
{
    public interface IInvTopsseInvoiceAppService : IApplicationService
    {
        Task<PagedResultDto<InvTopsseInvoiceDto>> GetTopsseInvoiceSearch(GetInvTopsseInvoiceInput input);

        Task<PagedResultDto<InvTopsseInvoiceDetailsDto>> GetTopsseInvoiceDetailsById(GetInvTopsseInvoiceDetailsInput input);

    }
}
