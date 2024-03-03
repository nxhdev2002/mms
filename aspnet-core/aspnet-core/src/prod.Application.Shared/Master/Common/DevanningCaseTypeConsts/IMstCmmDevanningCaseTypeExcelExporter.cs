using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.Common.Dto;

namespace prod.Master.Common.Exporting
{

    public interface IMstCmmDevanningCaseTypeExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstCmmDevanningCaseTypeDto> mstcmmdevanningcasetype);
        FileDto ExportToHistoricalFile(List<string> data);


    }

}



