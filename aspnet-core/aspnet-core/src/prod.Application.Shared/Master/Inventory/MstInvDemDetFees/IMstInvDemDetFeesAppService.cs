using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{ 
    public interface IMstInvDemDetFeesAppService : IApplicationService
    {
        Task<PagedResultDto<MstInvDemDetFeesDto>> GetAll(GetMstInvDemDetFeesInput input);
        Task CreateOrEdit(CreateOrEditMstInvDemDetFeesDto input);
        Task Delete(EntityDto input);
        Task<List<MstInvDemDetFeesImportDto>> ImportMstInvDemDetFeesFromExcel(byte[] fileBytes, string fileName);
    }
}
