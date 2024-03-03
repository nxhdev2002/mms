using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.Gps.Mapping.Dto;
using System.Collections.Generic;

namespace prod.Inventory.Gps.Mapping.Exporting
{
    public interface IInvGpsMappingExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvGpsMappingDto> data);
    }
}
