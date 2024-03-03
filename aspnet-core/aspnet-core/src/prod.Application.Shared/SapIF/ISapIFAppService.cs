using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.SapIF.Dto;
using System.Security.Cryptography;

namespace prod.SapIF
{
    public interface ISapIFAppService : IApplicationService
    {
        Task<OnlineBudgetCheckResponseDto> SapOnlineBudgetCheck(OnlineBudgetCheckRequest document);
        Task<SapIFResponseDto<FundCommitmentResponseDto>> SubmitFundCommitmentToSap(string type, List<long> listId, string action, bool? isClosed = null);
    }
}
