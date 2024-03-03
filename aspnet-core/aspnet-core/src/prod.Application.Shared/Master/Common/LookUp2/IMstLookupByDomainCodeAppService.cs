using Abp.Application.Services.Dto;
using prod.Master.Common.LookUp2.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Common.LookUp2
{
    public  interface IMstLookupByDomainCodeAppService
    {
        Task<PagedResultDto<GetMstLookUpForViewDto>> GetAll(GetMstLookUpInput input);
        Task<GetMstLookUpForOutput> GetMstLookUpEdit(EntityDto<long> input);
        Task Delete(EntityDto<long> input);
        Task CreateOrEdit(CreateOrEditMstLookUpDto input);
    }
}
