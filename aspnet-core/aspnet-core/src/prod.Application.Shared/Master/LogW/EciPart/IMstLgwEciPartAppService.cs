using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;
using prod.Master.LogW.EciPart.ImportDto;

namespace prod.Master.LogW
{

    public interface IMstLgwEciPartAppService : IApplicationService
    {

        Task<PagedResultDto<MstLgwEciPartDto>> GetAll(GetMstLgwEciPartInput input);

        Task CreateOrEdit(CreateOrEditMstLgwEciPartDto input);

        Task Delete(EntityDto input);

        Task<List<ImportMstLgwEciPartDto>> ImportEciPartFromExcel(List<ImportMstLgwEciPartDto> eicParts);

        Task MergeDataEciPart(string v_Guid);

    }

}


