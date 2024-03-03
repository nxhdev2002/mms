using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.Dto;
using prod.Master.Inv.Dto;
using prod.Master.Inventory.GpsSupplierOrderTime.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inv
{
    public interface IMstInvGpsSupplierOrderTimeAppService:IApplicationService
    {
        Task CreateOrEdit(CreateOrEditMstInvGpsSupplierOrderTimeDto input);

        Task Delete(EntityDto input);

    }
}
