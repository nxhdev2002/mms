using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Bar.Dto;

namespace prod.LogA.Bar.Exporting
{

    public interface ILgaBarScanInfoExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<LgaBarScanInfoDto> lgabarscaninfo);

    }

}