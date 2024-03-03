using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.Gps.StockIssuingTransDetails.Dto;
using prod.Inventory.GPS.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.StockIssuingTransDetails
{
    public interface IInvGpsStockIssuingTransactionAppService : IApplicationService
    {
        Task<PagedResultDto<InvGpsStockIssuingTransactionDto>> GetAll(GetStockIssuingTransactionInput input);

        Task<List<InvGpsStockIssuingTransactionDto>> ImportInvGpsStockReceivingTransFromExcel(byte[] fileBytes, string fileName);
    }
}
