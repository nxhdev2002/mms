using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.PartRobbing.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public interface IInvCkdSmqdOrderAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdSmqdOrderDto>> GetAll(GetInvCkdSmqdOrderInput input);

        Task<List<InvCkdSmqdOrderImportDto>> ImportSmqdOrderNormalFromExcel(byte[] fileBytes, string fileName);

        Task MergeDataSmqdOrderNormal(string v_Guid, int v_OrderType);

        Task<PagedResultDto<InvCkdSmqdOrderImportDto>> GetMessageErrorImport(string v_Guid);
    }
   
}