using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.IF.Dto;
using prod.Inventory.IF.FQF3MM04.Dto;

namespace prod.Inventory.IF.Exporting
{

    public interface IIF_FQF3MM04ExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<IF_FQF3MM04Dto> if_fqf3mm04);
        FileDto ExportValidateToFile(List<GetIF_FQF3MM04_VALIDATE> fqf3mm04);

    }

}


