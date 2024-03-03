using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.LogW.Dto;

namespace prod.LogW.Plc.Signal.Exporting
{
    public interface ILgwPlcSignalExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<LgwPlcSignalDto> lgwplcsignal);
    }
}
