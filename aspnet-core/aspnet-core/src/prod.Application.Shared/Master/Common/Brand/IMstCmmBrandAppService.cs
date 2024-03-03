using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Common.Dto;
using System.Threading.Tasks;

namespace prod.Master.Common
{

    public interface IMstCmmBrandAppService : IApplicationService
    {

        Task<PagedResultDto<MstCmmBrandDto>> GetAll(GetMstCmmBrandInput input);

        //Task CreateOrEdit(CreateOrEditMstCmmBrandDto input);

        //Task Delete(EntityDto input);

    }

}

