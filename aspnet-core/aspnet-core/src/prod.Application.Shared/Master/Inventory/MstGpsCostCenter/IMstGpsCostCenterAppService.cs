using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    public interface IMstGpsCostCenterAppService : ITransientDependency
    {
        Task<PagedResultDto<MstGpsCostCenterDto>> GetAll(GetMstGpsCostCenterInput input);
        Task<List<MstGpsCostCenterImportDto>> ImportGpsCostCenterFromExcel(byte[] fileBytes, string fileName);
    }
}
