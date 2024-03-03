using Abp.Application.Services.Dto;
using prod.Master.Pio.Dto; 
using Abp.Application.Services;
using System.Threading.Tasks;

namespace prod.Master.Pio
{
    public interface IMstPioImpSupplierAppService : IApplicationService
    {
        Task<PagedResultDto<MstPioImpSupplierDto>> GetAll(GetMstPioImpSupplierInput input);

    }
}
