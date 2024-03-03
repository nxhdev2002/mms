using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdPhysicalStockPeriodAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdPhysicalStockPeriodDto>> GetAll(GetInvCkdPhysicalStockPeriodInput input);

        Task CreateOrEdit(CreateOrEditInvCkdPhysicalStockPeriodDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<InvCkdPhysicalStockPeriodDto>> GetIdInvPhysicalStockPeriod();

    }

}

