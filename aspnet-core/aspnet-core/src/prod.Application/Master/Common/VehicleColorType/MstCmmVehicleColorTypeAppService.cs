using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_Master_Common_VehicleColorType_View)]
    public class MstCmmVehicleColorTypeAppService : prodAppServiceBase, IMstCmmVehicleColorTypeAppService
    {
        private readonly IDapperRepository<MstCmmVehicleColorType, long> _dapperRepo;
        private readonly IRepository<MstCmmVehicleColorType, long> _repo;
        private readonly IMstCmmVehicleColorTypeExcelExporter _calendarListExcelExporter;

        public MstCmmVehicleColorTypeAppService(IRepository<MstCmmVehicleColorType, long> repo,
                                         IDapperRepository<MstCmmVehicleColorType, long> dapperRepo,
                                        IMstCmmVehicleColorTypeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstCmmVehicleColorTypeDto>> GetAll(GetMstCmmVehicleColorTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmVehicleColorTypeDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmVehicleColorTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetVehicleColorTypeToExcel(MstCmmVehicleColorTypeExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstCmmVehicleColorTypeDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
