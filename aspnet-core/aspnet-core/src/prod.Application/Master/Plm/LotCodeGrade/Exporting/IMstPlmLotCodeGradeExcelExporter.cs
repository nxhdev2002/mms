using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.Plm.Dto;


namespace prod.Master.Plm.Exporting
{

    public interface IMstPlmLotCodeGradeExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstPlmLotCodeGradeDto> mstplmlotcodegrade);

    }

}


