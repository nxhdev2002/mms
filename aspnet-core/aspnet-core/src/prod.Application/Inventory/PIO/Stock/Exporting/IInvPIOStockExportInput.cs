using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.PIO.Stock.Dto;

namespace prod.Inventory.PIO.Stock.Exporting
{

    public interface IInvPIOStockExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvPIOStockDto> invpiostock);

    }

}


