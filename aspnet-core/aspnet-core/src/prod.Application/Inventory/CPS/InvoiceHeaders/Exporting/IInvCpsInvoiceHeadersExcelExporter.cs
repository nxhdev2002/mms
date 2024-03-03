using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Inventory.CPS.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inventory.CPS.Exporting
{

    public interface IInvCpsInvoiceHeadersExcelExporter : IApplicationService
    {
        FileDto ExportToFileHeaders(List<InvCpsInvoiceHeadersGrid> invcpsinvoiceheaders);

        FileDto ExportToFileLines(List<InvCpsInvoiceLinesDtoGrid> invcpsinvoicelines);


    }

}