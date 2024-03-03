using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.LogA.Ekb.Dto;
using prod.LogA.Ekb.Ekanban.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.LogA.Ekb.Ekanban
{
    public interface ILgaEkbEkanbanAppService : IApplicationService
    {
        Task<PagedResultDto<LgaEkbEkanbanDto>> GetAll(GetLgaEkbEkanbanInput input);

        Task CreateOrEdit(CreateOrEditLgaEkbEkanbanDto input);

        Task Delete(EntityDto input);
    }
}
