using Abp.Application.Services;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.VehicleCBU.Dto;
using System.Collections.Generic;

namespace prod.Master.Common.VehicleCBU.Exporting
{
    public interface IMstCmmVehicleCBUExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstCmmVehicleCBUDto> mstcmmvehiclecbu);

        FileDto ExportToFileColor(List<MstCmmVehicleCBUColorDto> mstcmmvehiclecbucolor);
        FileDto ExportToFileValitate(List<MstCmmVehicleCBUColorValidationResultDto> mstcmmvehiclecbucolorvalidationresult);
        FileDto ExportToHistoricalFile(List<string> data);

    }
}
