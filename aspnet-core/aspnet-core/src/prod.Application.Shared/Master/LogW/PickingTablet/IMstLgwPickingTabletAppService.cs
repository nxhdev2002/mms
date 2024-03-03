using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW.PickingTablet
{

    public interface IMstLgwPickingTabletAppService : IApplicationService
    {

        Task<PagedResultDto<MstLgwPickingTabletDto>> GetAll(GetMstLgwPickingTabletInput input);

        Task CreateOrEdit(CreateOrEditMstLgwPickingTabletDto input);

        Task Delete(EntityDto input);
        Task<FileDto> GetPickingTabletToExcel(MstLgwPickingTabletExportInput input);


    }

}


