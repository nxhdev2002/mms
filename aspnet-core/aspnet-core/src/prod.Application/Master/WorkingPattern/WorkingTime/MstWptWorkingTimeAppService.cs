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
using prod.Master.WorkingPattern.Dto;
using prod.Master.WorkingPattern.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.WorkingPattern
{
    [AbpAuthorize(AppPermissions.Pages_WorkingPattern_WorkingTime_View)]
    public class MstWptWorkingTimeAppService : prodAppServiceBase, IMstWptWorkingTimeAppService
    {
        private readonly IDapperRepository<MstWptWorkingTime, long> _dapperRepo;
        private readonly IRepository<MstWptWorkingTime, long> _repo;
        private readonly IMstWptWorkingTimeExcelExporter _calendarListExcelExporter;

        public MstWptWorkingTimeAppService(IRepository<MstWptWorkingTime, long> repo,
                                         IDapperRepository<MstWptWorkingTime, long> dapperRepo,
                                        IMstWptWorkingTimeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_WorkingTime_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstWptWorkingTimeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstWptWorkingTimeDto input)
        {

            var mainObj = ObjectMapper.Map<MstWptWorkingTime>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);

        }

        // EDIT
        private async Task Update(CreateOrEditMstWptWorkingTimeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }

        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_WorkingTime_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstWptWorkingTimeDto_Dapper>> GetAll(GetMstWptWorkingTimeInput_Dapper input)
        {
            string _sql = "Exec MST_WPT_WORKING_TIME_SEARCH @SHIFT_NO, @SHOP_NAME";

            var filtered = await _dapperRepo.QueryAsync<MstWptWorkingTimeDto_Dapper>(_sql, new
            {
                SHIFT_NO = input.ShiftNo,
                SHOP_NAME = input.ShopName
            });

            var results = from o in filtered
                          select new MstWptWorkingTimeDto_Dapper
                          {
                              Id = o.Id,
                              ShiftNo = o.ShiftNo,
                              ShopId = o.ShopId,
                              ShopName = o.ShopName,
                              StartTime = o.StartTime,
                              EndTime = o.EndTime,
                              WorkingType = o.WorkingType,
                              WorkingTypeDesc = o.WorkingTypeDesc,
                              Description = o.Description,
                              IsActive = o.IsActive,
                              StartDate = o.StartDate,
                              EndDate = o.EndDate,
                              PatternHId = o.PatternHId,
                              PatternDescription = o.PatternDescription,
                              SeasonType = o.SeasonType,
                              DayOfWeek = o.DayOfWeek,
                              SeasonDayOfWeek = o.SeasonDayOfWeek,
                              WeekWorkingDays = o.WeekWorkingDays,
                              RowNo = o.RowNo

                          };

            var totalCount = filtered.ToList().Count;
            var paged = results.AsQueryable().PageBy(input);

            return new PagedResultDto<MstWptWorkingTimeDto_Dapper>(
                totalCount,
                paged.ToList()
            );
        }

        public async Task<PagedResultDto<MstWptPatternDDto>> MstWptPatternD_GetsActive()
        {
            string _sql = "Exec MST_WPT_PATTERN_D_GETS_ACTIVE";

            var filtered = await _dapperRepo.QueryAsync<MstWptPatternDDto>(_sql, new { });

            var results = from d in filtered
                          select new MstWptPatternDDto
                          {
                              Id = d.PatternHId,
                              ShiftNo = d.ShiftNo,
                              ShiftName = d.ShiftName,
                              StartTime = d.StartTime,
                              EndTime = d.EndTime

                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<MstWptPatternDDto>(
                totalCount,
                results.ToList()
            );
        }

        public async Task<PagedResultDto<MstWptShopDto>> MstWptShop_GetsActive()
        {
            string _sql = "Exec MST_WPT_SHOP_GETS_ACTIVE @Id";

            var filtered = await _dapperRepo.QueryAsync<MstWptShopDto>(_sql, new
            {
                Id = ""
            });

            var results = from s in filtered
                          select new MstWptShopDto
                          {
                              Id = s.Id,
                              ShopName = s.ShopName,
                              Description = s.Description

                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<MstWptShopDto>(
                totalCount,
                results.ToList()
            );
        }


        public async Task<FileDto> GetWorkingTimeToExcel(MstWptWorkingTimeExportInput input)
        {
            string _sql = "Exec MST_WPT_WORKING_TIME_SEARCH @SHIFT_NO, @SHOP_NAME";

            var filtered = await _dapperRepo.QueryAsync<MstWptWorkingTimeDto_Dapper>(_sql, new
            {
                SHIFT_NO = input.ShiftNo,
                SHOP_NAME = input.ShopName
            });

            var query = from o in filtered
                        select new MstWptWorkingTimeDto
                        {
                            Id = o.Id,
                            ShiftNo = o.ShiftNo,
                            ShopId = o.ShopId,
                            WorkingType = o.WorkingType,
                            StartTime = o.StartTime,
                            EndTime = o.EndTime,
                            Description = o.Description,
                            PatternHId = o.PatternHId,
                            SeasonType = o.SeasonType,
                            DayOfWeek = o.DayOfWeek,
                            WeekWorkingDays = o.WeekWorkingDays,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = query.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
