using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;
using prod.Dto;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD.Exporting
{

    public interface IInvCkdProdPlanDailyExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvCkdProdPlanDailyDto> invckdProdPlanDaily);
        FileDto ExportToHistoricalFile(List<string> data);
        
    }

}

