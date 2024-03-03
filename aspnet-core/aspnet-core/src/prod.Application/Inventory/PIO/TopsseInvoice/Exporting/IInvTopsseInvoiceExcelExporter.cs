using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.PIO.TopsseInvoice.Dto;
using System.Collections.Generic;

namespace prod.Inventory.PIO.TopsseInvoice.Exporting
{
    public interface IInvTopsseInvoiceExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvTopsseInvoiceDto> invtopsseinvoice);

        FileDto ExportToFileDetails(List<InvTopsseInvoiceDetailsDto> invtopsseinvoicedetails);

    }
}
