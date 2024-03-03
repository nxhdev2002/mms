using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Inventory.CPS.Dto;
using System.Collections.Generic;

namespace prod.Inventory.CPS.Exporting
{

    public interface IInvCpsPoHeadersExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<GridPoHeadersDto> invcpspoheaders);

        FileDto ExportToFilePoline(List<InvCpsPoLinesDto> polines);

    }

}



