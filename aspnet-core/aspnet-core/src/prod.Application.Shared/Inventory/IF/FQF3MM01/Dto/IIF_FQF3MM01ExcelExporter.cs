using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;
using prod.Inventory.IF.FQF3MM01.Dto;

namespace prod.Inventory.IF.FQF3MM01.Exporting
{

    public interface IIF_FQF3MM01ExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<IF_FQF3MM01Dto> if_fqf3mm01);
        FileDto ExportValidateToFile(List<GetIF_FQF3MM01_VALIDATE> fqf3mm01);

    }

}

