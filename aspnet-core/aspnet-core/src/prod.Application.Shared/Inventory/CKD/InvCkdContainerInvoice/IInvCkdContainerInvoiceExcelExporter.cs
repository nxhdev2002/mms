using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD.Exporting
{

    public interface IInvCkdContainerInvoiceExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvCkdContainerInvoiceDto> invckdcontainerinvoice);

        FileDto ExportCustomsToExcel(List<InvCkdContainerInvoiceCustomsDto> invckdcontainerinvoicecustoms);
        FileDto ExportViewCustomsToExcel(List<InvCkdContainerInvoiceViewCustomsDto> invckdcontainerinvoiceviewcustoms);
        FileDto ExportToHistoricalFile(List<string> data);
             
    }

}


