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
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_ScreenConfig)]
    public class MstLgwScreenConfigAppService : prodAppServiceBase, IMstLgwScreenConfigAppService
    {
        private readonly IDapperRepository<MstLgwScreenConfig, long> _dapperRepo;
        private readonly IRepository<MstLgwScreenConfig, long> _repo;
        private readonly IMstLgwScreenConfigExcelExporter _calendarListExcelExporter;

        public MstLgwScreenConfigAppService(IRepository<MstLgwScreenConfig, long> repo,
                                         IDapperRepository<MstLgwScreenConfig, long> dapperRepo,
                                        IMstLgwScreenConfigExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_ScreenConfig_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgwScreenConfigDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgwScreenConfigDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgwScreenConfig>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgwScreenConfigDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_ScreenConfig_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgwScreenConfigDto>> GetAll(GetMstLgwScreenConfigInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.UnpackDoneColor), e => e.UnpackDoneColor.Contains(input.UnpackDoneColor))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NeedToUnpackColor), e => e.NeedToUnpackColor.Contains(input.NeedToUnpackColor))
                               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwScreenConfigDto
                         {
                             Id = o.Id,
                             UnpackDoneColor = o.UnpackDoneColor,
                             NeedToUnpackColor = o.NeedToUnpackColor,
                             NeedToUnpackFlash = o.NeedToUnpackFlash,
                             ConfirmFlagColor = o.ConfirmFlagColor,
                             ConfirmFlagSound = o.ConfirmFlagSound,
                             ConfirmFlagPlaytime = o.ConfirmFlagPlaytime,
                             ConfirmFlagFlash = o.ConfirmFlagFlash,
                             DelayUnpackColor = o.DelayUnpackColor,
                             DelayUnpackSound = o.DelayUnpackSound,
                             DelayUnpackPlaytime = o.DelayUnpackPlaytime,
                             DelayUnpackFlash = o.DelayUnpackFlash,
                             CallLeaderColor = o.CallLeaderColor,
                             CallLeaderSound = o.CallLeaderSound,
                             CallLeaderPlaytime = o.CallLeaderPlaytime,
                             CallLeaderFlash = o.CallLeaderFlash,
                             TotalColumnOldShift = o.TotalColumnOldShift,
                             TotalColumnSeqA1 = o.TotalColumnSeqA1,
                             TotalColumnSeqA2 = o.TotalColumnSeqA2,
                             BeforeTacktimeColor = o.BeforeTacktimeColor,
                             BeforeTacktimeSound = o.BeforeTacktimeSound,
                             BeforeTacktimePlaytime = o.BeforeTacktimePlaytime,
                             BeforeTacktimeFlash = o.BeforeTacktimeFlash,
                             TackCaseA1 = o.TackCaseA1,
                             TackCaseA2 = o.TackCaseA2,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwScreenConfigDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetScreenConfigToExcel(MstLgwScreenConfigExportInput input)
        {
            var filtered = _repo.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.UnpackDoneColor), e => e.UnpackDoneColor.Contains(input.UnpackDoneColor))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.NeedToUnpackColor), e => e.NeedToUnpackColor.Contains(input.NeedToUnpackColor))
                                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwScreenConfigDto
                        {
                            Id = o.Id,
                            UnpackDoneColor = o.UnpackDoneColor,
                            NeedToUnpackColor = o.NeedToUnpackColor,
                            NeedToUnpackFlash = o.NeedToUnpackFlash,
                            ConfirmFlagColor = o.ConfirmFlagColor,
                            ConfirmFlagSound = o.ConfirmFlagSound,
                            ConfirmFlagPlaytime = o.ConfirmFlagPlaytime,
                            ConfirmFlagFlash = o.ConfirmFlagFlash,
                            DelayUnpackColor = o.DelayUnpackColor,
                            DelayUnpackSound = o.DelayUnpackSound,
                            DelayUnpackPlaytime = o.DelayUnpackPlaytime,
                            DelayUnpackFlash = o.DelayUnpackFlash,
                            CallLeaderColor = o.CallLeaderColor,
                            CallLeaderSound = o.CallLeaderSound,
                            CallLeaderPlaytime = o.CallLeaderPlaytime,
                            CallLeaderFlash = o.CallLeaderFlash,
                            TotalColumnOldShift = o.TotalColumnOldShift,
                            TotalColumnSeqA1 = o.TotalColumnSeqA1,
                            TotalColumnSeqA2 = o.TotalColumnSeqA2,
                            BeforeTacktimeColor = o.BeforeTacktimeColor,
                            BeforeTacktimeSound = o.BeforeTacktimeSound,
                            BeforeTacktimePlaytime = o.BeforeTacktimePlaytime,
                            BeforeTacktimeFlash = o.BeforeTacktimeFlash,
                            TackCaseA1 = o.TackCaseA1,
                            TackCaseA2 = o.TackCaseA2,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstLgwScreenConfigConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
