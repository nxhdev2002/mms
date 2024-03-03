using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA
{

    public interface IMstLgaEkbProcessAppService : IApplicationService
    {

        Task<PagedResultDto<MstLgaEkbProcessDto>> GetAll(GetMstLgaEkbProcessInput input);

        Task CreateOrEdit(CreateOrEditMstLgaEkbProcessDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetEkbProcessToExcel(GetMstLgaEkbProcessExcelInput input);

    }

}


