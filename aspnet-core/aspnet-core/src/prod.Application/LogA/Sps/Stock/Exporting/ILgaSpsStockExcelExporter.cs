using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Sps.Dto;

namespace prod.LogA.Sps.Exporting
{

    public interface ILgaSpsStockExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<LgaSpsStockDto> lgaspsstock);

    }

}


