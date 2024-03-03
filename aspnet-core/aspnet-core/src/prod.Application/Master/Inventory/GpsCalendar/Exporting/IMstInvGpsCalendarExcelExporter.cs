using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Inv.Dto;
using prod.Dto;
using prod.Master.Inv.Dto;

namespace prod.Master.Inv.Exporting
{

    public interface IMstInvGpsCalendarExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvGpsCalendarDto> mstinvgpscalendar);

    }

}