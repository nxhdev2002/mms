using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.WorkingPattern.Dto;
using prod.Master.WorkingPattern.Exporting;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.WorkingPattern
{
    [AbpAuthorize(AppPermissions.Pages_WorkingPattern_Calendar_View)]
    public class MstWptCalendarAppService : prodAppServiceBase, IMstWptCalendarAppService
    {
        private readonly IDapperRepository<MstWptCalendar, long> _dapperRepo;
        private readonly IRepository<MstWptCalendar, long> _repo;
        private readonly IMstWptCalendarExcelExporter _calendarListExcelExporter;

        public MstWptCalendarAppService(IRepository<MstWptCalendar, long> repo,
                                         IDapperRepository<MstWptCalendar, long> dapperRepo,
                                        IMstWptCalendarExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_Calendar_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstWptCalendarDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstWptCalendarDto input)
        {
            var chk = await _repo.FirstOrDefaultAsync(e => e.WorkingDate == input.WorkingDate);

            if (chk != null)
            {
                throw new UserFriendlyException(400, L("Ngày làm việc đã tồn tại")); // TODO: Change Message
            }
            var mainObj = ObjectMapper.Map<MstWptCalendar>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstWptCalendarDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_Calendar_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstWptCalendarDto>> GetAll(GetMstWptCalendarInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll()
                        .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                        .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom <= t.WorkingDate)
                        .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo >= t.WorkingDate);

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate);

            var system = from o in pageAndFiltered
                         select new MstWptCalendarDto
                         {
                             Id = o.Id,
                             WorkingDate = o.WorkingDate,
                             WorkingType =
                            (
                                o.WorkingType == "0" ? "HOLIDAY" :
                                o.WorkingType == "123" ? "ALL DAY" :
                                o.WorkingType == "1" ? "SHIFT 1 WORK ONLY" :
                                o.WorkingType == "2" ? "SHIFT 2 WORK ONLY" :
                                o.WorkingType == "3" ? "SHIFT 3 WORK ONLY" :
                                o.WorkingType == "12" ? "SHIFT 1,2 WORK ONLY" :
                                o.WorkingType == "13" ? "SHIFT 1,3 WORK ONLY" :
                                o.WorkingType == "23" ? "SHIFT 2,3 WORK ONLY" : "Unknown"
                            ),
                             WorkingStatus = o.WorkingStatus,
                             DayOfWeek = o.DayOfWeek,
                             SeasonType =
                            (
                                o.SeasonType == "L" ? "Low" :
                                o.SeasonType == "H" ? "High" :
                                o.SeasonType == "N" ? "Normal" : "Unknown"
                            ),
                             WeekNumber = o.WeekNumber,
                             WeekWorkingDays = o.WeekWorkingDays,
                             IsActive =
                            (
                                o.WorkingDate < dateTime ? "Y" : "N"
                            )
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);


            return new PagedResultDto<MstWptCalendarDto>(
                totalCount,
                await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCalendarToExcel(MstWptCalendarExportInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll()
                        .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                        .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom <= t.WorkingDate)
                        .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo >= t.WorkingDate);

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate);

            var query = from calendars in pageAndFiltered
                        select new MstWptCalendarDto
                        {
                            Id = calendars.Id,
                            WorkingDate = calendars.WorkingDate,
                            WorkingType = calendars.WorkingType,
                            WorkingStatus = calendars.WorkingStatus,
                            DayOfWeek = calendars.DayOfWeek,
                            SeasonType = calendars.SeasonType,
                            WeekNumber = calendars.WeekNumber,
                            WeekWorkingDays = calendars.WeekWorkingDays,
                            IsActive = calendars.IsActive
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task GenerateAsync()
        {
            await _dapperRepo.ExecuteAsync(MstWptCalendarConsts.SP_MST_WPT_CALENDAR_GENERATE);
        }

        // View By Month
        //public async Task<PagedResultDto<MstWptCalendarDetailDto>> Get_Pivot_Month(GetMstWptCalendarDetailInput input)
        //{
        //    DateTime dateTime = DateTime.UtcNow.Date;
        //    var filtered = _repo.GetAll();

        //    var calendar = from o in filtered
        //                    .WhereIf(input.monthSearch.HasValue, t => t.WorkingDate.Month == input.monthSearch.Value.Month + 1 )
        //                    .WhereIf(input.yearSearch.HasValue, t => t.WorkingDate.Year == input.yearSearch.Value.Year)
        //                    .WhereIf(!input.yearSearch.HasValue, t => t.WorkingDate.Year == dateTime.Year)
        //                   .Where(c => c.IsActive == "Y")
        //    select new MstWptCalendarDetailDto
        //                   {
        //                       WorkingType =
        //                    (
        //                        o.WorkingType == "0" ? "Off" :
        //                        o.WorkingType == "123" ? "S1,2,3" :
        //                        o.WorkingType == "1" ? "S1" :
        //                        o.WorkingType == "2" ? "S2" :
        //                        o.WorkingType == "3" ? "S3" :
        //                        o.WorkingType == "12" ? "S1" :
        //                        o.WorkingType == "23" ? "S2" :
        //                        o.WorkingType == "23" ? "S3" : "Unknown"
        //                    ),
        //                       //Working_Month = (new DateTime(o.WorkingDate.Year, o.WorkingDate.Month, o.WorkingDate.Day).AddDays(-((o.WorkingDate.Day) - 1))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
        //                       Working_Month = o.WorkingDate,
        //                       Day_No = "DAY_" + o.WorkingDate.Day,
        //                       IsActive = o.IsActive
        //                   };

        //    var pivotMonth = calendar
        //        .GroupBy(c => new { Month = c.Working_Month.Value.Month, Year = c.Working_Month.Value.Year })
        //        .Select(g => new MstWptCalendarDetailDto
        //        {

        //            //Working_Month = calendar.
        //            Working_Month = g.Min(e => e.Working_Month),
        //            DAY_1 = g.Where(c => c.Day_No == "DAY_1").Max(c => c.WorkingType),
        //            DAY_2 = g.Where(c => c.Day_No == "DAY_2").Max(c => c.WorkingType),
        //            DAY_3 = g.Where(c => c.Day_No == "DAY_3").Max(c => c.WorkingType),
        //            DAY_4 = g.Where(c => c.Day_No == "DAY_4").Max(c => c.WorkingType),
        //            DAY_5 = g.Where(c => c.Day_No == "DAY_5").Max(c => c.WorkingType),
        //            DAY_6 = g.Where(c => c.Day_No == "DAY_6").Max(c => c.WorkingType),
        //            DAY_7 = g.Where(c => c.Day_No == "DAY_7").Max(c => c.WorkingType),
        //            DAY_8 = g.Where(c => c.Day_No == "DAY_8").Max(c => c.WorkingType),
        //            DAY_9 = g.Where(c => c.Day_No == "DAY_9").Max(c => c.WorkingType),
        //            DAY_10 = g.Where(c => c.Day_No == "DAY_10").Max(c => c.WorkingType),
        //            DAY_11 = g.Where(c => c.Day_No == "DAY_11").Max(c => c.WorkingType),
        //            DAY_12 = g.Where(c => c.Day_No == "DAY_12").Max(c => c.WorkingType),
        //            DAY_13 = g.Where(c => c.Day_No == "DAY_13").Max(c => c.WorkingType),
        //            DAY_14 = g.Where(c => c.Day_No == "DAY_14").Max(c => c.WorkingType),
        //            DAY_15 = g.Where(c => c.Day_No == "DAY_15").Max(c => c.WorkingType),
        //            DAY_16 = g.Where(c => c.Day_No == "DAY_16").Max(c => c.WorkingType),
        //            DAY_17 = g.Where(c => c.Day_No == "DAY_17").Max(c => c.WorkingType),
        //            DAY_18 = g.Where(c => c.Day_No == "DAY_18").Max(c => c.WorkingType),
        //            DAY_19 = g.Where(c => c.Day_No == "DAY_19").Max(c => c.WorkingType),
        //            DAY_20 = g.Where(c => c.Day_No == "DAY_20").Max(c => c.WorkingType),
        //            DAY_21 = g.Where(c => c.Day_No == "DAY_21").Max(c => c.WorkingType),
        //            DAY_22 = g.Where(c => c.Day_No == "DAY_22").Max(c => c.WorkingType),
        //            DAY_23 = g.Where(c => c.Day_No == "DAY_23").Max(c => c.WorkingType),
        //            DAY_24 = g.Where(c => c.Day_No == "DAY_24").Max(c => c.WorkingType),
        //            DAY_25 = g.Where(c => c.Day_No == "DAY_25").Max(c => c.WorkingType),
        //            DAY_26 = g.Where(c => c.Day_No == "DAY_26").Max(c => c.WorkingType),
        //            DAY_27 = g.Where(c => c.Day_No == "DAY_27").Max(c => c.WorkingType),
        //            DAY_28 = g.Where(c => c.Day_No == "DAY_28").Max(c => c.WorkingType),
        //            DAY_29 = g.Where(c => c.Day_No == "DAY_29").Max(c => c.WorkingType),
        //            DAY_30 = g.Where(c => c.Day_No == "DAY_30").Max(c => c.WorkingType),
        //            DAY_31 = g.Where(c => c.Day_No == "DAY_31").Max(c => c.WorkingType),
        //        }).OrderBy(e => e.Working_Month) ;


        //    var totalCount = await pivotMonth.CountAsync();
        //    var paged = pivotMonth.PageBy(input);

        //    return new PagedResultDto<MstWptCalendarDetailDto>(
        //      totalCount,
        //      await paged.ToListAsync()
        //  );

        //}

        // view by month
        public async Task<PagedResultDto<MstWptCalendarDetailDto>> Get_Pivot_Month(GetMstWptCalendarDetailInput input)
        {
            string _sql = "Exec MST_WPT_CALENDAR_VIEW_BY_MONTH @WORKING_YEAR ,@WORKING_MONTH";

            var filtered = await _dapperRepo.QueryAsync<MstWptCalendarDetailDto>(_sql, new
            {

                WORKING_YEAR = input.yearSearch,
                WORKING_MONTH = input.monthSearch
            });

            var pageAndFiltered = filtered.OrderBy(s => s.WorkingMonth);


            var results = from d in pageAndFiltered
                          select new MstWptCalendarDetailDto
                          {
                              WorkingMonth = d.WorkingMonth,
                              WorkingType = d.WorkingType,
                              Day_No = d.Day_No,
                              IsActive = d.IsActive,

                              //DAY
                              DAY_1 = d.DAY_1,
                              DAY_2 = d.DAY_2,
                              DAY_3 = d.DAY_3,
                              DAY_4 = d.DAY_4,
                              DAY_5 = d.DAY_5,
                              DAY_6 = d.DAY_6,
                              DAY_7 = d.DAY_7,
                              DAY_8 = d.DAY_8,
                              DAY_9 = d.DAY_9,
                              DAY_10 = d.DAY_10,
                              DAY_11 = d.DAY_11,
                              DAY_12 = d.DAY_12,
                              DAY_13 = d.DAY_13,
                              DAY_14 = d.DAY_14,
                              DAY_15 = d.DAY_15,
                              DAY_16 = d.DAY_16,
                              DAY_17 = d.DAY_17,
                              DAY_18 = d.DAY_18,
                              DAY_19 = d.DAY_19,
                              DAY_20 = d.DAY_20,
                              DAY_21 = d.DAY_21,
                              DAY_22 = d.DAY_22,
                              DAY_23 = d.DAY_23,
                              DAY_24 = d.DAY_24,
                              DAY_25 = d.DAY_25,
                              DAY_26 = d.DAY_26,
                              DAY_27 = d.DAY_27,
                              DAY_28 = d.DAY_28,
                              DAY_29 = d.DAY_29,
                              DAY_30 = d.DAY_30,
                              DAY_31 = d.DAY_31,
                          };

            var totalCount = filtered.ToList().Count;
            var paged = results.AsQueryable().PageBy(input);

            return new PagedResultDto<MstWptCalendarDetailDto>(
                totalCount,
                paged.ToList()
            );
        }
    }
}

