using Abp.Application.Services;
using prod.Dto;
using prod.Master.Common.Dto;
using System.Collections.Generic;

namespace prod.Master.Common.MMValidationResult.Exporting
{
    public interface IMstCmmMMValidationResultExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstCommonMMValidationResultDto> mstcmmmmvalidationresult);

        FileDto ExportToHistoricalFile(List<string> data);

    }
}
