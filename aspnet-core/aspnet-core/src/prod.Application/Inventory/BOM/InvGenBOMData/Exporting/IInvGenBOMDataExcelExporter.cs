
using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.BOM.Dto;
using System.Collections.Generic;

namespace prod.Inventory.BOM.Exporting
{

    public interface IInvGenBOMDataExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvGenBOMDataDto> invgenbomdatat);

    }

}