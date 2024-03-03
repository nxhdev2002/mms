using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;

using prod.Inv.Dmr.Dto;

namespace prod.Inv.Dmr.Exporting
{

    public interface IInvImportByContExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvImportByContDto> invimportbycont);

    }

}


