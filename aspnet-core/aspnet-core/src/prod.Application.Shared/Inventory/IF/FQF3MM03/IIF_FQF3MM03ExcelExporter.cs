using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.Inventory.IF.FQF3MM03.Dto;

namespace prod.Inventory.IF.Exporting
{

    public interface IIF_FQF3MM03ExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<IF_FQF3MM03Dto> if_fqf3mm03);
        FileDto ExportValidateToFile(List<GetIF_FQF3MM03_VALIDATE> fqf3mm03);

    }

}


