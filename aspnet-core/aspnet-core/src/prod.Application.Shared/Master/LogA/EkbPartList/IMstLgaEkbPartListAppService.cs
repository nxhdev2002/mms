using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;
namespace prod.Master.LogA
{
    public interface IMstLgaEkbPartListAppService : IApplicationService
    {
        Task<PagedResultDto<MstLgaEkbPartListDto>> GetAll(GetMstLgaEkbPartListInput input);
        Task CreateOrEdit(CreateOrEditMstLgaEkbPartListDto input);
        Task Delete(EntityDto input);

        Task<FileDto> GetEkbPartListToExcel(GetMstLgaEkbPartListExcelInput input);
    }

}
