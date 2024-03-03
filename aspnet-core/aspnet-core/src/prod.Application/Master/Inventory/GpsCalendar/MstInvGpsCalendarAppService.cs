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
using prod.Master.Inv.Dto;
using prod.Master.Inv.Exporting;

namespace prod.Master.Inv
{
    [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsCalendar_View)]
    public class MstInvGpsCalendarAppService : prodAppServiceBase, IMstInvGpsCalendarAppService
    {
        private readonly IDapperRepository<MstInvGpsCalendar, long> _dapperRepo;
        private readonly IRepository<MstInvGpsCalendar, long> _repo;
        private readonly IMstInvGpsCalendarExcelExporter _calendarListExcelExporter;

        public MstInvGpsCalendarAppService(IRepository<MstInvGpsCalendar, long> repo,
                                         IDapperRepository<MstInvGpsCalendar, long> dapperRepo,
                                        IMstInvGpsCalendarExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsCalendar_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstInvGpsCalendarDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvGpsCalendarDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvGpsCalendar>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvGpsCalendarDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsCalendar_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstInvGpsCalendarDto>> GetAll(GetMstInvGpsCalendarInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCode), e => e.SupplierCode.Contains(input.SupplierCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.WorkingType), e => e.WorkingType.Contains(input.WorkingType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.WorkingStatus), e => e.WorkingStatus.Contains(input.WorkingStatus))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate);
            ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate);


            var system = from o in pageAndFiltered
                         select new MstInvGpsCalendarDto
                         {
                             Id = o.Id,
                             SupplierCode = o.SupplierCode,
                             WorkingDate = o.WorkingDate,
                             WorkingType = o.WorkingType,
                             WorkingStatus = o.WorkingStatus,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvGpsCalendarDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetGpsCalendarToExcel(MstInvGpsCalendarExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate); ;

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate);

            var query = from o in pageAndFiltered
                         select new MstInvGpsCalendarDto
                        {
                            Id = o.Id,
                            SupplierCode = o.SupplierCode,
                            WorkingDate = o.WorkingDate,
                            WorkingType = o.WorkingType,
                            WorkingStatus = o.WorkingStatus,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstInvGpsCalendarConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
