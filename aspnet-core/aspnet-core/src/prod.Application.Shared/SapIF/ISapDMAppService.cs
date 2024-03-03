using Abp.Application.Services;
using prod.SapIF.Dto;
using System.Threading.Tasks;

namespace prod.SapIF
{
    public interface ISapDMAppService : IApplicationService
    {
        Task<SapIFResponseDto<FundCommitmentResponseDto>> SubmitFundCommitmentDMToSap();
    }
}
