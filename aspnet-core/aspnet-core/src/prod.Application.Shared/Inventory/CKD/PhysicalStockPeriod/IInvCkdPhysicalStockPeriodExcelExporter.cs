using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD.Exporting
{

    public interface IInvCkdPhysicalStockPeriodExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvCkdPhysicalStockPeriodDto> invckdphysicalstockperiod);

    }

}

