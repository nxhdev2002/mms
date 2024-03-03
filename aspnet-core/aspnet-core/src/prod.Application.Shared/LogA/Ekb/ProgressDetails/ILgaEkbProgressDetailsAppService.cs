using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Ekb.Dto;

namespace prod.LogA.Ekb
{
    public interface ILgaEkbProgressDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<LgaEkbProgressDetailsDto>> GetAll(GetLgaEkbProgressDetailsInput input);
        Task CreateOrEdit(CreateOrEditLgaEkbProgressDetailsDto input);
        Task Delete(EntityDto input);
    }
}