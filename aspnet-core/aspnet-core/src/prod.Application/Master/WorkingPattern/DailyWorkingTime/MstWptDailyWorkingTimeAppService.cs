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
using prod;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.WorkingPattern;
using prod.Master.WorkingPattern.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Exporting;

namespace prod.Master.WorkingPattern
{
    [AbpAuthorize(AppPermissions.Pages_WorkingPattern_DailyWorkingTime_View)]
    public class MstWptDailyWorkingTimeAppService : prodAppServiceBase, IMstWptDailyWorkingTimeAppService
    {
        private readonly IDapperRepository<MstWptDailyWorkingTime, long> _dapperRepo;
        private readonly IDapperRepository<MstWptPatternD, long> _dapperRePatternD;
        private readonly IRepository<MstWptDailyWorkingTime, long> _repo;
        private readonly IRepository<MstWptShop, long> _repoShop;
        private readonly IRepository<MstWptWorkingType, long> _repoWorkingType;

        private readonly IMstWptDailyWorkingTimeExcelExporter _calendarListExcelExporter;

        public MstWptDailyWorkingTimeAppService(IRepository<MstWptDailyWorkingTime, long> repo,
                                            IRepository<MstWptShop, long> repoShop,
                                            IRepository<MstWptWorkingType, long> repoWorkingType,
                                            IRepository<MstWptPatternH, long> repoPatternH,
                                            IDapperRepository<MstWptDailyWorkingTime, long> dapperRepo,
                                             IDapperRepository<MstWptPatternD, long> dapperRePatternD,
                                            IMstWptDailyWorkingTimeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _repoShop = repoShop;
            _repoWorkingType = repoWorkingType;
            _dapperRepo = dapperRepo;
            _dapperRePatternD = dapperRePatternD;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_DailyWorkingTime_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstWptDailyWorkingTimeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstWptDailyWorkingTimeDto input)
        {
            await _dapperRepo.ExecuteAsync(@"UPDATE SrvQuoInvoiceDetail SET IsDeleted = 1, DeletionTime = GETDATE(), DeleterUserId = @UserId WHERE InvCusId = @InvoiceId", new
            {
                InvoiceId = input.Id,
                UserId = AbpSession.UserId
            });
        }

        private async Task<int> Update(CreateOrEditMstWptDailyWorkingTimeDto input)
        {
            try
            {
                string _sql = @"EXEC MST_WPT_DAILY_WORKING_TIME _UPDATE @id, @SHIFT_NO, @SHOP_ID, @WORKING_DATE, @START_TIME, @END_TIME, @WORKING_TYPE, @DESCRIPTION, @IS_ACTIVE";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    id = input.Id,
                    SHIFT_NO = input.ShiftNo,
                    SHOP_ID = input.ShiftName,
                    WORKING_DATE = input.WorkingDate,
                    START_TIME = input.StartTime,
                    END_TIME = input.EndTime,
                    WORKING_TYPE = input.WorkingType,
                    DESCRIPTION = input.Description,
                    IS_ACTIVE = input.IsActive
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_DailyWorkingTime_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstWptDailyWorkingTimeDto>> GetAll(GetMstWptDailyWorkingTimeInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            string _sql = "Exec MST_WPT_DAILY_WORKING_TIME_SEARCH @SHIFT_NO, @SHOP_NAME,@WORKING_TYPE,@WORKING_DATE_FROM,@WORKING_DATE_TO";

            var filtered = await _dapperRepo.QueryAsync<MstWptDailyWorkingTimeDto>(_sql, new
            {
                SHIFT_NO = input.ShiftNo,
                SHOP_NAME = input.ShopName,
                WORKING_TYPE = input.WorkingType,
                WORKING_DATE_FROM = input.WorkingDateFrom,
                WORKING_DATE_TO = input.WorkingDateTo
            });

            var results = from d in filtered
                         select new MstWptDailyWorkingTimeDto
                         {
                             Id = d.Id,
                             ShiftNo = d.ShiftNo,
                             ShiftName = d.ShiftName,
                             ShopId = d.ShopId,
                             ShopName = d.ShopName,
                             WorkingDate = d.WorkingDate,
                             StartTime = d.StartTime,
                             EndTime = d.EndTime,
                             WorkingType = d.WorkingType,
                             WorkingTypeDesc = d.WorkingTypeDesc,
                             Description = d.Description,
                             IsActive = d.IsActive,
                             FromTime = d.FromTime,
                             ToTime = d.ToTime,
                             IsManual = d.IsManual
                         };
                       

            
            var totalCount = filtered.ToList().Count;
            var paged = results.AsQueryable().PageBy(input);

            return new PagedResultDto<MstWptDailyWorkingTimeDto>(
                totalCount,
                paged.ToList()
            );
        }

        public async Task<PagedResultDto<MstWptPatternDDto>> MstWptPatternD_GetsActive()
        {
            string _sql = "Exec MST_WPT_PATTERN_D_GETS_ACTIVE";

            var filtered = await _dapperRepo.QueryAsync<MstWptPatternDDto>(_sql, new{});

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

        public async Task<PagedResultDto<MstWptShopDto>>MstWptShop_GetsActive()
        {
            string _sql = "Exec MST_WPT_SHOP_GETS_ACTIVE @Id";

            var filtered = await _dapperRepo.QueryAsync<MstWptShopDto>(_sql, new {
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

        public async Task<PagedResultDto<MstWptWorkingTypeDto>> MstWptWorkingType_GetsForDaily()
        {
            string _sql = "Exec MST_WPT_WORKING_TYPE_GETS_FOR_DAILY";

            var filtered = await _dapperRepo.QueryAsync<MstWptWorkingTypeDto>(_sql, new { });

            var results = from d in filtered
                          select new MstWptWorkingTypeDto
                          {
                              Id = d.Id,
                              WorkingType = d.WorkingType,
                              Description = d.Description

                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<MstWptWorkingTypeDto>(
                totalCount,
                results.ToList()
            );
        }


        public async Task<FileDto> GetDailyWorkingTimeToExcel(MstWptDailyWorkingTimeExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            string _sql = "Exec MST_WPT_DAILY_WORKING_TIME_SEARCH @SHIFT_NO, @SHOP_NAME,@WORKING_TYPE,@WORKING_DATE_FROM,@WORKING_DATE_TO";

            var filtered = await _dapperRepo.QueryAsync<MstWptDailyWorkingTimeDto>(_sql, new
            {
                SHIFT_NO = input.ShiftNo,
                SHOP_NAME = input.ShopName,
                WORKING_TYPE = input.WorkingType,
                WORKING_DATE_FROM = input.WorkingDateFrom,
                WORKING_DATE_TO = input.WorkingDateTo
            });

            var query = from o in filtered
                          select new MstWptDailyWorkingTimeDto
                        {
                            Id = o.Id,
                            ShiftNo = o.ShiftNo,
                            ShiftName = o.ShiftName,
                            ShopId = o.ShopId,
                            WorkingDate = o.WorkingDate,
                            StartTime = o.StartTime,
                            EndTime = o.EndTime,
                            WorkingType = o.WorkingType,
                            Description = o.Description,
                            FromTime = o.FromTime,
                            ToTime = o.ToTime,
                            IsManual = o.IsManual,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = query.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstWptDailyWorkingTimeConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
