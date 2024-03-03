using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using prod.Inventory.CPS.Dto;
using prod.Master.Common.GradeColor.Dto;
using System.Collections.Generic;
using prod.Dto;

namespace prod.Inventory.CPS
{

    public interface IInvCpsPoHeadersAppService : IApplicationService
    {

        Task<PagedResultDto<GridPoHeadersDto>> GetCpsPoHeaderSearch(GetInvCpsPoHeadersInput input);
        Task<FileDto> GetPoLinesToExcel(long? p_id);

        Task<List<CbxInventoryGroupDto>> GetCbxInventoryGroup();

        Task<List<CbxSupplierDto>> GetCbxSupplier();

    }

}


