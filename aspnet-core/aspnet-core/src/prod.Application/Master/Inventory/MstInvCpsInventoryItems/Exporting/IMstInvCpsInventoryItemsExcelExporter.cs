using System;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.Inventory.Dto;

namespace prod.Master.Inventory.Exporting
{

    public interface IMstInvCpsInventoryItemsExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvCpsInventoryItemsDto> mstinvcpsinventoryitems);

    }
}