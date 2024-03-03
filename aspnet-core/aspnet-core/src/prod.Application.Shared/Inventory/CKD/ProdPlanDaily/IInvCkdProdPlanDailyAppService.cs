using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{

    public interface IInvCkdProdPlanDailyAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdProdPlanDailyDto>> GetAll(GetInvCkdProdPlanDailyInput input);

        Task<FileDto> GetExportReportDaily(InvCkdProductionPlanDailyReportInput input);

        Task<FileDto> GetExportReportMonthly(InvCkdProductionPlanDailyReportInput input);

    }

}


