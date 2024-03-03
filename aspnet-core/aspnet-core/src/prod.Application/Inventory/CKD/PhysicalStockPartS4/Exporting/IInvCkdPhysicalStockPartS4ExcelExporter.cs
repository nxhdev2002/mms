using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;
using prod.Inv.CKD.Dto;

namespace prod.Inventory.CKD.Exporting
{
    public interface IInvCkdPhysicalStockPartS4ExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdPhysicalStockPartS4Dto> invckdmodulecase);       
    }
}
