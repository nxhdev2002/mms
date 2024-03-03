using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.PIO.StockReceiving.Dto;
using prod.Inventory.PIO.StockTransaction.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.StockReceiving.Exporting
{
    public interface IInvPIOStockReceivingExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvPIOStockReceivingDto> invpiostockreceiving);
    }

}
