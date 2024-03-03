using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CPS.Dto;
using prod.Inventory.CPS.SapAssetMaster.Dto;
using prod.Inventory.SPP.InvoiceDetails.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CPS.SapAssetMaster
{
    public interface IInvCpsSapAssetMasterAppService : IApplicationService
    {
        Task<PagedResultDto<InvCpsSapAssetMasterDto>> GetAll(GetInvCpsSapAssetMasterInput input);
    }
}
