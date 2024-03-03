using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD.Exporting
{

    public interface IInvCkdProductionMappingExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvCkdProductionMappingDto> invckdproductionmapping);

        FileDto ExportToFileErr(List<InvCkdProductionMappingDto> listimporterr);
        FileDto ExportToHistoricalFile(List<string> data);

    }

}


