using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inv.CKD.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inv.CKD
{
    public interface IInvCkdPhysicalStockPartS4AppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdPhysicalStockPartS4Dto>> GetAll(GetInvCkdPhysicalStockPartS4Input input);
        Task<List<InvCkdPhysicalStockPartS4Dto>> ImportDataPhysicalStockCPartS4FromExcel(byte[] fileBytes, string fileName);
    }
}