using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.CKD.Dto;

namespace prod.Master.CKD
{

    public interface IMstCkdCustomsLeadtimeAppService : IApplicationService
    {

        Task<PagedResultDto<MstCkdCustomsLeadtimeDto>> GetAll(GetMstCkdCustomsLeadtimeInput input);

        //Task CreateOrEdit(CreateOrEditMstCkdCustomsLeadtimeDto input);

        //Task Delete(EntityDto input);

    }

}


