using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inventory.Dto;
using System.Collections.Generic;

namespace prod.Master.Inventory.Exporting
{

    public interface IMstInvCpsInventoryGroupExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvCpsInventoryGroupDto> mstinvcpsinventorygroup);

    }

}

