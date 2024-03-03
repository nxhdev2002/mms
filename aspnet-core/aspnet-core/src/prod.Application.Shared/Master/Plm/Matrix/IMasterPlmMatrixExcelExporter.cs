using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Plm.Dto;

namespace prod.Master.Plm
{

    public interface IMasterPlmMatrixExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MasterPlmMatrixDto> masterplmmatrix);

    }

}
