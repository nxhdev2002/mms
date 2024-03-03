using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Inv.Dto;
using prod.Inventory.CKD.Invoice.Dto;
using prod.Inventory.Invoice.Dto;

namespace prod.Master.Inventory
{

    public interface IMstInvGpsSupplierInfoAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvGpsSupplierInfoDto>> GetAllSupplierInfo(GetMstInvGpsSupplierInfoInput input);

        Task CreateOrEdit(CreateOrEditMstInvGpsSupplierInfoDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<MstInvGpsSupplierOrderTimeDto>> GetSupplierOrderTimeBySupplierId(GetMstInvGpsSupplierOrderTimeInput input);
    }

}