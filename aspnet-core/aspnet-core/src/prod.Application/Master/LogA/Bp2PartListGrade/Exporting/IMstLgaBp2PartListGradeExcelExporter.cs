using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Bp2PartListGrade.Dto;

namespace prod.Master.LogA.Exporting
{
    public interface IMstLgaBp2PartListGradeExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstLgaBp2PartListGradeDto> mstlgabp2partlistgrade);
    }
}
