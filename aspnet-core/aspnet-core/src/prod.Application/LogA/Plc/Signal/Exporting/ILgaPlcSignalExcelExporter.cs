using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Pcs.Dto;
using prod.LogA.Plc.Signal.Dto;

namespace prod.LogA.Plc.Exporting
{
    public interface ILgaPlcSignalExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<LgaPlcSignalDto> lgaplcsignal);
    }
}
