using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW.Exporting
{
    public interface IMstLgwAdoCallingLightExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstLgwAdoCallingLightDto> mstlgwadocallinglight);

    }

}
