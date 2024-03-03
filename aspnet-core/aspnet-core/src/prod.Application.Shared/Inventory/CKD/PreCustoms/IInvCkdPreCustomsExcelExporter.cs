using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Dto;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD.Exporting
{

    public interface IInvCkdPreCustomsExcelExporter : IApplicationService
    {

        FileDto PreCustomsExportToFile(List<InvCkdPreCustomsDto> invckdprecustoms);

        FileDto InvoiceExportToFile(List<InvoiceListDto> invckdinvoice);

        FileDto InvoiceDetailExportToFile(List<InvoiceDetailListDto> invckinvoicedetail);

    }

}


