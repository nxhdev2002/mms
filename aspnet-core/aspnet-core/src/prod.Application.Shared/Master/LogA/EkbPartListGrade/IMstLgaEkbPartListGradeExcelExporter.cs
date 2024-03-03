using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA.Exporting
{

    public interface IMstLgaEkbPartListGradeExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstLgaEkbPartListGradeDto> mstlgaekbpartlistgrade);

    }

}