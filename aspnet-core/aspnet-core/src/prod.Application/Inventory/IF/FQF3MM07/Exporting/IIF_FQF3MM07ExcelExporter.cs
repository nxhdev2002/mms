using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.IF.Dto;
using prod.Inventory.IF.FQF3MM07.Dto;

namespace prod.Inventory.IF.FQF3MM07.Exporting
{

    public interface IIF_FQF3MM07ExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<IF_FQF3MM07Dto> if_fqf3mm07);
        FileDto ExportItemDMToFile(List<GetIF_FundCommitmentItemDMExportDto> if_FundCommitmentItemDM);

        FileDto ExportValidateToFile(List<GetIF_FQF3MM07_VALIDATE> if_fqf3mm07);
    }

}


