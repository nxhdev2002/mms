using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.Cmm.Dto;

namespace prod.Master.Cmm.Exporting
{

    public interface IMstCmmEngineMasterExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstCmmEngineMasterDto> mstcmmenginemaster);

        FileDto ExportToHistoricalFile(List<string> data);

    }

}