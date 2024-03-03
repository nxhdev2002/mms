using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

using prod.Dto;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD.Exporting
{

    public interface IInvCkdContainerTransitPortPlanExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvCkdContainerTransitPortPlanDto> invckdcontainertransitportplan);
        FileDto ExportToFileErr(List<InvCkdContainerTransitPortPlanDto> invckdcontainertransitportplan_err);
    }

}


