using Abp.Application.Services;
using prod.Dto;
using prod.Master.WorkingPattern.Dto;
using System;
using System.Collections.Generic;

namespace prod.Master.WorkingPattern.Exporting
{
    public interface IMstWptCalendarExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstWptCalendarDto> calendar);

    }


}