using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.PIO.StockTransaction.Dto;
using System.Collections.Generic;

namespace prod.Inventory.PIO.StockTransaction.Exporting
{
    public interface IInvPIOStockTransactionExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvPIOStockTransactionDto> invpiostocktransaction);

    }
}
