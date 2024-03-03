using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Dto;
using prod.Master.Cmm.Dto;

namespace prod.Master.Cmm.Exporting
{

    public interface IMstCmmEngineModelExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstCmmEngineModelDto> mstcmmenginemodel);

    }

}