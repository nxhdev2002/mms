using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Plm.Dto;

namespace prod.Master.Plm.Exporting
{

    public interface IMasterPlmPartExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MasterPlmPartDto> masterplmpart);

    }

}