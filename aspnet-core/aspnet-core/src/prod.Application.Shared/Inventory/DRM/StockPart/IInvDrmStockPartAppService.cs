using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.DRM.Dto;
using prod.Inventory.DRM.StockPart.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.DRM.StockPart
{
    public interface IInvDrmStockPartAppService : IApplicationService
    {
        Task<PagedResultDto<InvDrmStockPartDto>> GetAll(GetInvDrmStockPartInput input);

        Task<List<InvDrmStockPartImportDto>> ImportInvDRMStockPartFromExcel(byte[] fileBytes, string fileName);

    }
}
