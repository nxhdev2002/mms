using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Cmm.Dto;
using System.Threading.Tasks;


namespace prod.Master.Cmm
{

    public interface IMstCmmEngineModelAppService : IApplicationService
    {
        Task<PagedResultDto<MstCmmEngineModelDto>> GetAll(GetMstCmmEngineModelInput input);

    }

}

