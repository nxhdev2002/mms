using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.GPS.Dto;
using prod.Dto;

namespace prod.Inventory.GPS.Exporting
{

    public interface IInvGpsStockRundownExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvGpsStockRundownDto> invgpsstockrundown);

    }

}


