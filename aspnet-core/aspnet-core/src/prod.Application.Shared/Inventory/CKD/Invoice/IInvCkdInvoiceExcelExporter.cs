using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Invoice.Dto;
using prod.Inventory.Invoice.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.Invoice
{
    public interface IInvCkdInvoiceExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdInvoiceDto> invinvoice);

        FileDto ExportToFile2(List<InvCkdInvoiceDetailsDto> invinvoice2);

        FileDto ExportToFile3(List<InvCkdInvoiceCustomsDto> invinvoice3);


        FileDto ExportToFileByLotNo(List<InvCkdInvoiceDetailsByLotDto> invoicedetails);

        FileDto ExportToHistoricalFile(List<string> data, string tablename);

    }
}
