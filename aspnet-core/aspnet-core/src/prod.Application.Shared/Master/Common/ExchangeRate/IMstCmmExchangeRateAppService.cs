using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using prod.Master.Common.Dto;

namespace prod.Master.Common
{
    public interface IMstCmmExchangeRateAppService : IApplicationService
    {
        Task<PagedResultDto<MstCmmExchangeRateDto>> GetAll(GetMstCmmExchangeRateInput input);
        Task<PagedResultDto<MstCmmExchangeRateDiffDto>> GetDataDiffExchangeRate(MstCmmExchangeRateDiffDtoInput input);
    }

}