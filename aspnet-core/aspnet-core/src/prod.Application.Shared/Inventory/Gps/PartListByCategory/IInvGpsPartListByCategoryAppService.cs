using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.Gps.PartListByCategory.Dto;
using prod.Inventory.GPS.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.PartListByCategory
{
    public interface IInvGpsPartListByCategoryAppService : IApplicationService
    {
        Task<PagedResultDto<InvGpsPartListByCategoryDto>> GetAll(GetInvGpsPartListByCategoryInput input);
        Task<List<InvGpsPartListByCategoryImportDto>> ImportInvGpsPartListByCategoryFromExcel(byte[] fileBytes, string fileName);
    }
}
