using Abp.Application.Services;
using prod.Dto;
using prod.Master.Common.CarSeries.Dto;
using prod.Master.Common.Dto;
using System.Collections.Generic;

namespace prod.Master.Common.CarSeries.Exporting
{
    public interface IMstCmmCarSeriesExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstCmmCarSeriesDto> mstcmmcarseries);
    }
}
