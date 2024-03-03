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
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_ValuationType_View)]
    public class MstCmmValuationTypeAppService : prodAppServiceBase, IMstCmmValuationTypeAppService
    {
        private readonly IDapperRepository<MstCmmValuationType, long> _dapperRepo;
        private readonly IRepository<MstCmmValuationType, long> _repo;
        private readonly IMstCmmValuationTypeExcelExporter _calendarListExcelExporter;

        public MstCmmValuationTypeAppService(IRepository<MstCmmValuationType, long> repo,
                                         IDapperRepository<MstCmmValuationType, long> dapperRepo,
                                        IMstCmmValuationTypeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstCmmValuationTypeDto>> GetAll(GetMstCmmValuationTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                //.WhereIf(!string.IsNullOrWhiteSpace(input.MaterialType), e => e.MaterialType.Contains(input.MaterialType))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new MstCmmValuationTypeDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             //Product = o.Product,
                             //MaterialTypeId = o.MaterialTypeId,
                             //MaterialType = o.MaterialType,
                             //IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmValuationTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetValuationTypeToExcel(MstCmmValuationTypeExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
               //.WhereIf(!string.IsNullOrWhiteSpace(input.MaterialType), e => e.MaterialType.Contains(input.MaterialType))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new MstCmmValuationTypeDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             //Product = o.Product,
                             //MaterialTypeId = o.MaterialTypeId,
                             //MaterialType = o.MaterialType,
                             //IsActive = o.IsActive,
                         };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
