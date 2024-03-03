using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inventory.Dto;
using System.Collections.Generic;

namespace prod.Master.Inventory.Exporting
{
    public interface IMstGpsMaterialRegisterByShopExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstGpsMaterialRegisterByShopDto> mstGpsMaterialRegisterByShops);
    }
}
