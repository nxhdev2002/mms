using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.GPS.Dto;

namespace prod.Inventory.GPS.Exporting
{

    public interface IInvGpsMaterialExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvGpsMaterialDto> invgpsMaterial);

        FileDto ExportToFileErr(List<ImportInvGpsMaterialDto> impErr);
        FileDto ExportToHistoricalFile(List<string> data);

    }

}


