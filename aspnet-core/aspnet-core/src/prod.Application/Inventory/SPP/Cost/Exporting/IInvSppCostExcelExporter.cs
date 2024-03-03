using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.SPP.Cost.Dto;
using System.Collections.Generic;


namespace prod.Inventory.SPP.Cost.Exporting
{
    public interface IInvSppCostExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvSppCostDto> invsppcost);
    }
}
