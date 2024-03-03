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
    [AbpAuthorize(AppPermissions.Pages_Master_Cmm_EngineModel_View)]
    public class MstCmmEngineModelAppService : prodAppServiceBase, IMstCmmEngineModelAppService
    {
        private readonly IDapperRepository<MstCmmEngineModel, long> _dapperRepo;
        private readonly IRepository<MstCmmEngineModel, long> _repo;
        private readonly IMstCmmEngineModelExcelExporter _calendarListExcelExporter;

        public MstCmmEngineModelAppService(IRepository<MstCmmEngineModel, long> repo,
                                         IDapperRepository<MstCmmEngineModel, long> dapperRepo,
                                        IMstCmmEngineModelExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }



        public async Task<PagedResultDto<MstCmmEngineModelDto>> GetAll(GetMstCmmEngineModelInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmEngineModelDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmEngineModelDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetEngineModelToExcel(MstCmmEngineModelExportInput input)
        {
            var filtered = _repo.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var system = from o in pageAndFiltered
                         select new MstCmmEngineModelDto
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
        //    await _dapperRepo.ExecuteAsync(MstCmmEngineModelConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
