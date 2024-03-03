using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.Gps.StockReceivingTransDetails.Dto;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    public interface IMstGpsMaterialRegisterByShopAppService : IApplicationService
    {
        Task<PagedResultDto<MstGpsMaterialRegisterByShopDto>> GetAll(GetMstGpsMaterialRegisterByShopInput input);
        Task<List<MstGpsMaterialRegisterByShopImportDto>> ImportMaterialRegisterByShopFromExcel(byte[] fileBytes, string fileName);
    }
}
