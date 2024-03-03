using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.Tmss.Dto;
using prod.Dto;
using prod.Inventory.Tmss.Dto;

namespace prod.Inventory.Tmss.Exporting
{

    public interface IInvTmssDispatchPlanExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvTmssDispatchPlanDto> invtmssdispatchplan);
        FileDto ExportToHistoricalFile(List<string> data);

    }

}


