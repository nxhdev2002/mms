using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.PIO.StockRundownTransaction.Dto;
using System.Collections.Generic;

namespace prod.Inventory.PIO.StockRundownTransaction.Exporting
{
    public interface IInvPIOStockRundownTransactionExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvPIOStockRundownTransactionDto> invpiostockrundowntransaction);

    }
}
