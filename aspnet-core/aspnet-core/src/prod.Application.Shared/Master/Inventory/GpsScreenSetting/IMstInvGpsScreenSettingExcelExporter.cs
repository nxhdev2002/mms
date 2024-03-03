using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Inv.Dto;

namespace prod.Master.Inv.Exporting
{

    public interface IMstInvGpsScreenSettingExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvGpsScreenSettingDto> mstinvgpsscreensetting);

    }

}


