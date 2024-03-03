using Abp.Application.Services;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.GradeColor.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Common.LotCodeGrade.Exporting
{
    public interface IMstCmmLotCodeGradeExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstCmmLotCodeGradeTDto> mstcmmlotcodegrade);
        FileDto ExportToHistoricalFile(List<string> data);

    }
}
