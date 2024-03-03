using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Cmm.Dto;
using prod.Dto;
using prod.Master.Cmm.Dto;

namespace prod.Master.Cmm.Exporting
{

    public interface IMstCmmColorExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstCmmColorDto> mstcmmcolor);
        FileDto ExportToHistoricalFile(List<string> data);

    }

}


