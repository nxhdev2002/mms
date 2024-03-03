using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Inventory
{
    public interface IMstInvDemDetFeesExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstInvDemDetFeesDto> mstinvdemdetfees);
        FileDto ExportToFileErr(List<MstInvDemDetFeesImportDto> mstinvdemdetfees_err);
    }
}
