using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.Inventory.GPS.Dto;

namespace prod.Inventory.GPS.Exporting
{

    public interface IInvGpsDailyOrderExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvGpsDailyOrderDto> invgpsdailyorder);

    }

}


