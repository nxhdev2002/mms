using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Vehicle.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.Vehicle
{
    public interface IInvCkdVehicleAppService
    {
        Task<PagedResultDto<InvCkdVehicleDto>> GetAll(InvCkdVehicleInput input);

        Task<FileDto> GetExportReportDaily(InvCkdProductionActualReportInput input);

        Task<FileDto> GetExportReportMonthly(InvCkdProductionActualReportInput input);
        Task Update(InvCkdVehicleDto input);
    }
}
