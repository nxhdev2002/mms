
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Painting.Dto;

namespace prod.Master.Painting
{

    public interface IMstPtsInventoryStdAppService : IApplicationService
    {

        Task<PagedResultDto<MstPtsInventoryStdDto>> GetAll(GetMstPtsInventoryStdInput input);

        Task CreateOrEdit(CreateOrEditMstPtsInventoryStdDto input);

        Task Delete(EntityDto input);

    }

}
