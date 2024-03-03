using Abp.Application.Services;
using prod.Dto;
using prod.Master.Cmm.Dto;
using System.Collections.Generic;

namespace prod.Master.Cmm.Exporting
{

    public interface IMstCmmMMCheckingRuleExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstCmmMMCheckingRuleDto> mstcmmmmcheckingrule);

        FileDto ExportToFileErr(List<MstCmmMMCheckingRuleImportDto> mmcheckingrule_err);

        FileDto ExportToHistoricalFile(List<string> data);

    }

}


