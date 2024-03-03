using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Cmm;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_Master_Common_VehicleName_View)]
    public class MstCmmVehicleNameAppService : prodAppServiceBase, IMstCmmVehicleNameAppService
    {
        private readonly IDapperRepository<MstCmmVehicle, long> _dapperRepo;
        private readonly IRepository<MstCmmVehicle, long> _repo;
        private readonly IMstCmmVehicleNameExcelExporter _calendarListExcelExporter;

        public MstCmmVehicleNameAppService(IRepository<MstCmmVehicle, long> repo,
                                         IDapperRepository<MstCmmVehicle, long> dapperRepo,
                                         IMstCmmVehicleNameExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstCmmVehicleNameDto>> GetAll(GetMstCmmVehicleNameInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.VehicleCode.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.VehicleName.Contains(input.Name))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmVehicleNameDto
                         {
                             Id = o.Id,
                             Code = o.VehicleCode,
                             Name = o.VehicleName,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmVehicleNameDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
        public async Task<FileDto> GetVehicleNameToExcel(MstCmmVehicleNameExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.VehicleCode.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.VehicleName.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var system = from o in pageAndFiltered
                         select new MstCmmVehicleNameDto
                        {
                            Id = o.Id,
                            Code = o.VehicleCode,
                            Name = o.VehicleName,
                        };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
