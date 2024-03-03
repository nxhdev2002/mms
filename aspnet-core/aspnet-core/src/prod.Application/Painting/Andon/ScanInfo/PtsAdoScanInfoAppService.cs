using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Painting.Andon.Dto;
using prod.Painting.Andon.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Painting.Andon
{
    [AbpAuthorize(AppPermissions.Pages_ProdPlan_ScanInfo_View)]
    public class PtsAdoScanInfoAppService : prodAppServiceBase, IPtsAdoScanInfoAppService
    {
        private readonly IDapperRepository<PtsAdoScanInfo, long> _dapperRepo;
        private readonly IRepository<PtsAdoScanInfo, long> _repo;
        private readonly IPtsAdoScanInfoExcelExporter _calendarListExcelExporter;

        public PtsAdoScanInfoAppService(IRepository<PtsAdoScanInfo, long> repo,
                                         IDapperRepository<PtsAdoScanInfo, long> dapperRepo,
                                        IPtsAdoScanInfoExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        // [AbpAuthorize(AppPermissions.Pages_ProdPlan_ScanInfo_Edit)]
        //public async Task CreateOrEdit(CreateOrEditPtsAdoScanInfoDto input)
        //{
        //    if (input.Id == null) await Create(input);
        //    else await Update(input);
        //}

        ////CREATE
        //private async Task Create(CreateOrEditPtsAdoScanInfoDto input)
        //{
        //    var mainObj = ObjectMapper.Map<PtsAdoScanInfo>(input);

        //    await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        //}

        //// EDIT
        //private async Task Update(CreateOrEditPtsAdoScanInfoDto input)
        //{
        //    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
        //    {
        //        var mainObj = await _repo.GetAll()
        //        .FirstOrDefaultAsync(e => e.Id == input.Id);

        //        var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
        //    }
        //}

        //  [AbpAuthorize(AppPermissions.Pages_ProdPlan_ScanInfo_Delete)]
        //public async Task Delete(EntityDto input)
        //{
        //    var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
        //    _repo.HardDelete(mainObj);
        //    CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        //}

        public async Task<PagedResultDto<PtsAdoScanInfoDto>> GetAll(GetPtsAdoScanInfoInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScanType), e => e.ScanType.Contains(input.ScanType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation), e => e.ScanLocation.Contains(input.ScanLocation))
                ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.ScanTime);



            var system = from o in pageAndFiltered
                         select new PtsAdoScanInfoDto
                         {
                             Id = o.Id,
                             ScanType = o.ScanType,
                             ScanValue = o.ScanValue,
                             ScanLocation = o.ScanLocation,
                             ScanTime = o.ScanTime,
                             ScanBy = o.ScanBy,
                             IsProcessed = o.IsProcessed,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<PtsAdoScanInfoDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetScanInfoToExcel(PtsAdoScanInfoExportInput input)
        {
            var filtered = _repo.GetAll()
                  .WhereIf(!string.IsNullOrWhiteSpace(input.ScanType), e => e.ScanType.Contains(input.ScanType))
                  .WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation), e => e.ScanLocation.Contains(input.ScanLocation))
                  ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                        select new PtsAdoScanInfoDto
                        {
                            Id = o.Id,
                            ScanType = o.ScanType,
                            ScanValue = o.ScanValue,
                            ScanLocation = o.ScanLocation,
                            ScanTime = o.ScanTime,
                            ScanBy = o.ScanBy,
                            IsProcessed = o.IsProcessed,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(PtsAdoScanInfoConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
