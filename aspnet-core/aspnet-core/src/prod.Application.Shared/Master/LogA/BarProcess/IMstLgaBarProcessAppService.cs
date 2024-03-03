using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA
{

    public interface IMstLgaBarProcessAppService : IApplicationService
    {

        Task<PagedResultDto<MstLgaBarProcessDto>> GetAll(GetMstLgaBarProcessInput input);

        Task CreateOrEdit(CreateOrEditMstLgaBarProcessDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetBarProcessToExcel(GetMstLgaBarProcesExcelInput input);


    }

}