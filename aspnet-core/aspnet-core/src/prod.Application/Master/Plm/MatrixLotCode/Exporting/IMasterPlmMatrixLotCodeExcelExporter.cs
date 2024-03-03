using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Plm.Dto;

namespace prod.Master.Plm.Exporting
{

    public interface IMasterPlmMatrixLotCodeExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MasterPlmMatrixLotCodeDto> masterplmmatrixlotcode);

    }

}


