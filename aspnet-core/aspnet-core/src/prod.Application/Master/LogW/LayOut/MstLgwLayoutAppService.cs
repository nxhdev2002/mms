using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.LogW.Dto;
using prod.Master.LogW.Exporting;

namespace prod.Master.LogW
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_Layout)]
    public class MstLgwLayoutAppService : prodAppServiceBase, IMstLgwLayoutAppService
    {
        private readonly IDapperRepository<MstLgwLayout, long> _dapperRepo;
        private readonly IRepository<MstLgwLayout, long> _repo;
        private readonly IMstLgwLayoutExcelExporter _calendarListExcelExporter;

        public MstLgwLayoutAppService(IRepository<MstLgwLayout, long> repo,
                                         IDapperRepository<MstLgwLayout, long> dapperRepo,
                                        IMstLgwLayoutExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }


        public async Task<PagedResultDto<MstLgwLayoutDto>> GetAll(GetMstLgwLayoutInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ZoneCd), e => e.ZoneCd.Contains(input.ZoneCd))
                .WhereIf(!string.IsNullOrWhiteSpace(input.AreaCd), e => e.AreaCd.Contains(input.AreaCd))
               
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwLayoutDto
                         {
                             Id = o.Id,
                             ZoneCd = o.ZoneCd,
                             AreaCd = o.AreaCd,
                             RowId = o.RowId,
                             ColumnId = o.ColumnId,
                             RowName = o.RowName,
                             ColumnName = o.ColumnName,
                             LocationCd = o.LocationCd,
                             LocationName = o.LocationName,
                             LocationTitle = o.LocationTitle,
                             IsDisabled = o.IsDisabled,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwLayoutDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetLayoutToExcel(MstLgwLayoutExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ZoneCd), e => e.ZoneCd.Contains(input.ZoneCd))
                .WhereIf(!string.IsNullOrWhiteSpace(input.AreaCd), e => e.AreaCd.Contains(input.AreaCd))

                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwLayoutDto
                        {
                            Id = o.Id,
                            ZoneCd = o.ZoneCd,
                            AreaCd = o.AreaCd,
                            RowId = o.RowId,
                            ColumnId = o.ColumnId,
                            RowName = o.RowName,
                            ColumnName = o.ColumnName,
                            LocationCd = o.LocationCd,
                            LocationName = o.LocationName,
                            LocationTitle = o.LocationTitle,
                            IsDisabled = o.IsDisabled,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstLgwLayoutConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
