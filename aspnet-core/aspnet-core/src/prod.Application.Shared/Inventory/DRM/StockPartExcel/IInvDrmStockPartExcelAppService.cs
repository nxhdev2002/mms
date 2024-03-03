using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.DRM.StockPartExcel.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inventory.DRM.StockPartExcel
{
    public interface IInvDrmStockPartExcelAppService : IApplicationService
    { 

        Task<List<InvDrmStockPartExcelImportDto>> ImportInvDRMStockPartExcelFromExcel(byte[] fileBytes, string fileName);

        Task<PagedResultDto<InvDrmStockPartExcelDetailDto>> GetInvDrmStockPartExcelDetail(InvDrmStockPartExcelDetailInputDto input);
        Task<PagedResultDto<InvDrmStockPartExcelDetailDto>> GetInvDrmStockPartExcelDetailSearch(InvDrmStockPartExcelDetailSearchInputDto input);
        Task<List<InvDrmStockPartDetailGridviewRowDto>> GetInvDrmStockPartStockUpdateTrans(InvDrmStockPartStockUpdateTransDto input);


    }
}
