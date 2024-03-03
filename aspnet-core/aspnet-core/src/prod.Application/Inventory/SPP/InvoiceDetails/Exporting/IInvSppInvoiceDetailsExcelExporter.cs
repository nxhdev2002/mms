using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.SPP.InvoiceDetails.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.SPP.InvoiceDetails.Exporting
{
    public interface IInvSppInvoiceDetailsExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvSppInvoiceDetailsDto> InvSppInvoiceDetails);
    }
}
