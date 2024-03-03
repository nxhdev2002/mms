using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD.Exporting
{

    public interface IInvCkdPhysicalStockPartExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvCkdPhysicalStockPartDto> invckdphysicalstockpart);

        FileDto ExportListErrToFile(List<InvCkdPhysicalStockErrDto> listerrphysicalstockpart);

        FileDto ExportListLotErrToFile(List<InvCkdPhysicalStockErrDto> listerrphysicalstocklot);

        FileDto ExportSummaryStockByPart(List<InvCkdPhysicalStockPartDto> listdata);
    }

}


