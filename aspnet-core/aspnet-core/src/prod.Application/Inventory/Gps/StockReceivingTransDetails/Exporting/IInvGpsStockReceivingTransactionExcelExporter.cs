using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.Gps.StockReceivingTransDetails.Dto;
using System.Collections.Generic;

namespace prod.Inventory.Gps.StockReceivingTransDetails.Exporting
{
    public interface IInvGpsStockReceivingTransactionExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvGpsStockReceivingTransactionDto> stockreceivingtrans);

        FileDto ExportErrToFile(List<InvGpsStockReceivingTransactionDto> stockReceivingtransErr);
    }
}
