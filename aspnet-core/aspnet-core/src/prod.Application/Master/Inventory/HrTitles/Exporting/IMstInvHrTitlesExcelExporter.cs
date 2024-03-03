using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inventory.HrTitles.Dto;
using prod.Master.Inventory.LotPart.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.HrTitles.Exporting
{
    public interface IMstInvHrTitlesExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstInvHrTitlesDto> mstinvhrtitles);

    }
}
