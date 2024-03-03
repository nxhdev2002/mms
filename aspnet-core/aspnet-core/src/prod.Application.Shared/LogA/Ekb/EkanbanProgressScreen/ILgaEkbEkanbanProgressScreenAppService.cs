using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Ekb.Dto;

namespace prod.LogA.Ekb
{

    public interface ILgaEkbEkanbanProgressScreenAppService : IApplicationService
    {   
        Task<List<LgaEkbEkanbanProgressScreenDto>> GetConfigScreen(string screen_id);

        Task<List<LgaEkbEkanbanProgressScreenDto>> GetDataEkabanProgressScreen(string prod_line);
    }

}