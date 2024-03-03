using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CPS.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inventory.CPS
{

    public interface IInvCpsInvoiceHeadersAppService : IApplicationService
    {
        Task<PagedResultDto<InvCpsInvoiceHeadersGrid>> GetInvoiceHeadersSearch(GetInvCpsInvoiceHeadersInput input);

        Task<PagedResultDto<InvCpsInvoiceLinesDtoGrid>> GetInvoiceLinesGetByInvoiceId(GetInvCpsInvoiceLinesInput input);

        Task<List<CbxInventoryGroup>> GetCbxInventoryGroup();

        Task<List<CbxSupplier>> GetCbxSupplier();
    }

}