using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.Gps.StockIssuingTransDetails.Dto;
using prod.Inventory.GPS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.StockIssuingTransDetails.Exporting
{
    public interface IInvGpsStockIssuingTransactionExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvGpsStockIssuingTransactionDto> stockissuingtrans);

        FileDto ExportErrToFile(List<InvGpsStockIssuingTransactionDto> stockissuingtransErr);
    }
}
