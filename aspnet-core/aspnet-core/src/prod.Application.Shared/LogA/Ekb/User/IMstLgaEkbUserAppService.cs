using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA
{

    public interface IMstLgaEkbUserAppService : IApplicationService
    {

        Task<PagedResultDto<MstLgaEkbUserDto>> GetAll(GetMstLgaEkbUserInput input);

        Task CreateOrEdit(CreateOrEditMstLgaEkbUserDto input);

        Task Delete(EntityDto input);

    }

}


