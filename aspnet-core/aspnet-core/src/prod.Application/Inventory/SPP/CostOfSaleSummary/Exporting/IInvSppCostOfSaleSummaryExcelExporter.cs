using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.SPP.CostOfSaleSummary.Dto;
using System.Collections.Generic;

namespace prod.Inventory.SPP.CostOfSaleSummary.Exporting
{
    public interface IInvSppCostOfSaleSummaryExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvSppCostOfSaleSummaryDto> invsppcostofsalesummary);

    }
}
