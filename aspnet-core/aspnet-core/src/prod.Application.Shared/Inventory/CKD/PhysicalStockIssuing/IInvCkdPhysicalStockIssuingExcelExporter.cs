using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;
using prod.Dto;
using prod.Inventory.CKD.PhysicalStockIssuing.Dto;

namespace prod.Inventory.CKD.Exporting
{

    public interface IInvCkdPhysicalStockIssuingExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdPhysicalStockIssuingDto> invckdphysicalstockissuing);

        FileDto ExportToFileDetailsData(List<InvCkdPhysicalStockIssuingDetailsDataDto> detailsdata);

        FileDto ExportReportSummaryToFile(List<InvCkdPhysicalStockIssuingReportSummartDto> invckdphysicalstockissuing);
    }

}


