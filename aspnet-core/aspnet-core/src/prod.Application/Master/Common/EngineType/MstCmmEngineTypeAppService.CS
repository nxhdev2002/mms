using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Cmm.Dto;
using prod.Master.Cmm.Exporting;
using prod.Master.Common.Dto;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Cmm
{
    [AbpAuthorize(AppPermissions.Pages_Master_Cmm_EngineType_View)]
    public class MstCmmEngineTypeAppService : prodAppServiceBase, IMstCmmEngineTypeAppService
    {
        private readonly IDapperRepository<MstCmmEngineType, long> _dapperRepo;
        private readonly IRepository<MstCmmEngineType, long> _repo;
        private readonly IMstCmmEngineTypeExcelExporter _calendarListExcelExporter;

        public MstCmmEngineTypeAppService(IRepository<MstCmmEngineType, long> repo,
                                         IDapperRepository<MstCmmEngineType, long> dapperRepo,
                                        IMstCmmEngineTypeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

    //    [AbpAuthorize(AppPermissions.Pages_Master_Cmm_EngineType_CreateEdit)]
        public async Task<PagedResultDto<MstCmmEngineTypeDto>> GetAll(GetMstCmmEngineTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmEngineTypeDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmEngineTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetEngineTypeToExcel(MstCmmEngineTypeExportInput input)
        {
            var filtered = _repo.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var system = from o in pageAndFiltered
                         select new MstCmmEngineTypeDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,

                         };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstCmmEngineTypeConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}