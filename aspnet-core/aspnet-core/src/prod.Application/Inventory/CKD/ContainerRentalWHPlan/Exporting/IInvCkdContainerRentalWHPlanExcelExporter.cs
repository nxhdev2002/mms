using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.Exporting
{

    public interface IInvCkdContainerRentalWHPlanExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvCkdContainerRentalWHPlanDto> invckdcontainerrentalwhplan);
        FileDto ExportToFileErr(List<InvCkdContainerRentalWHPlErrDto> invckdcontainerrentalwhplan_err);

    }   
}


