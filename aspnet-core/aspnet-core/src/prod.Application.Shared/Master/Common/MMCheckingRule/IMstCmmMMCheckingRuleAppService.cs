using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Cmm.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Master.Cmm
{
    public interface IMstCmmMMCheckingRuleAppService : IApplicationService
    {
        Task<PagedResultDto<MstCmmMMCheckingRuleDto>> GetAll(GetMstCmmMMCheckingRuleInput input);

        Task<List<MstCmmMMCheckingRuleImportDto>> ImportMstCmmMMCheckingRuleFromExcel(byte[] fileBytes, string fileName);


    }

}


