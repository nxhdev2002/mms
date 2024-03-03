using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.GPS.Dto;
using System.Collections.Generic;

namespace prod.Inventory.GPS.Exporting
{

    public interface IInvGpsReceivingExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvGpsReceivingDto> invgpsstockreceiving);

        FileDto ExportToFileErr(List<InvGpsReceivingImportDto> listimporterr);

    }

}


