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
    [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsScreenSetting_View)]
    public class MstInvGpsScreenSettingAppService : prodAppServiceBase, IMstInvGpsScreenSettingAppService
    {
        private readonly IDapperRepository<MstInvGpsScreenSetting, long> _dapperRepo;
        private readonly IRepository<MstInvGpsScreenSetting, long> _repo;
        private readonly IMstInvGpsScreenSettingExcelExporter _calendarListExcelExporter;

        public MstInvGpsScreenSettingAppService(IRepository<MstInvGpsScreenSetting, long> repo,
                                         IDapperRepository<MstInvGpsScreenSetting, long> dapperRepo,
                                        IMstInvGpsScreenSettingExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsScreenSetting_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstInvGpsScreenSettingDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvGpsScreenSettingDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvGpsScreenSetting>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvGpsScreenSettingDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsScreenSetting_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstInvGpsScreenSettingDto>> GetAll(GetMstInvGpsScreenSettingInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScreenName), e => e.ScreenName.Contains(input.ScreenName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScreenType), e => e.ScreenType.Contains(input.ScreenType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScreenValue), e => e.ScreenValue.Contains(input.ScreenValue))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvGpsScreenSettingDto
                         {
                             Id = o.Id,
                             ScreenName = o.ScreenName,
                             ScreenType = o.ScreenType,
                             ScreenValue = o.ScreenValue,
                             Description = o.Description,
                             BarcodeId = o.BarcodeId,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvGpsScreenSettingDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetGpsScreenSettingToExcel(GetMstInvGpsScreenSettingInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScreenName), e => e.ScreenName.Contains(input.ScreenName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScreenType), e => e.ScreenType.Contains(input.ScreenType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScreenValue), e => e.ScreenValue.Contains(input.ScreenValue))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var query = from o in pageAndFiltered
                        select new MstInvGpsScreenSettingDto
                        {
                            Id = o.Id,
                            ScreenName = o.ScreenName,
                            ScreenType = o.ScreenType,
                            ScreenValue = o.ScreenValue,
                            Description = o.Description,
                            BarcodeId = o.BarcodeId,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstInvGpsScreenSettingConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
