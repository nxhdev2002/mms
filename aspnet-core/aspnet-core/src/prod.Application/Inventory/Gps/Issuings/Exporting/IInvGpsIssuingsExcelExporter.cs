using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.Gps.Issuings.Dto;
using prod.Inventory.GPS.Dto;
using System.Collections.Generic;

namespace prod.Inventory.GPS.Exporting
{

    public interface IInvGpsIssuingsExcelExporter : IApplicationService
    {
        FileDto ExportToHistoricalFile(List<string> data);

    }

}


