using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using System.Collections.Generic;

namespace prod.Inventory.CKD
{
    public interface IInvCkdPartManagementExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvCkdPartManagementDto> invckdshipment);
        FileDto ExportReportToFile(List<InvCkdPartManagementReportDto> invckdshipment);
        FileDto ExportToHistoricalFile(List<string> data);

    }
}
