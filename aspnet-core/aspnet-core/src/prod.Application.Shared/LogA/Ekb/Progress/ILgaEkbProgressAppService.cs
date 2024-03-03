using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Ekb.Dto;

namespace prod.LogA.Ekb
{

    public interface ILgaEkbProgressAppService : IApplicationService
    {

        Task<PagedResultDto<LgaEkbProgressDto>> GetAll(GetLgaEkbProgressInput input);

        Task CreateOrEdit(CreateOrEditLgaEkbProgressDto input);

        Task Delete(EntityDto input);

    }

}