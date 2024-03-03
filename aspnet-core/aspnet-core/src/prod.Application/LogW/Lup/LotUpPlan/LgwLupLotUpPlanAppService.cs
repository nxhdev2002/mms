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
using prod.LogW.Lup.Dto;
using prod.LogW.Lup.Exporting;

namespace prod.LogW.Lup
{
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Lup_LotUpPlan)]
    public class LgwLupLotUpPlanAppService : prodAppServiceBase, ILgwLupLotUpPlanAppService
    {
        private readonly IDapperRepository<LgwLupLotUpPlan, long> _dapperRepo;
        private readonly IRepository<LgwLupLotUpPlan, long> _repo;
        private readonly ILgwLupLotUpPlanExcelExporter _calendarListExcelExporter;

        public LgwLupLotUpPlanAppService(IRepository<LgwLupLotUpPlan, long> repo,
                                         IDapperRepository<LgwLupLotUpPlan, long> dapperRepo,
                                        ILgwLupLotUpPlanExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Lup_LotUpPlan_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgwLupLotUpPlanDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgwLupLotUpPlanDto input)
        {
            var mainObj = ObjectMapper.Map<LgwLupLotUpPlan>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgwLupLotUpPlanDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Lup_LotUpPlan_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgwLupLotUpPlanDto>> GetAll(GetLgwLupLotUpPlanInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime).WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate);

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate)
                                        .ThenBy(s => s.ProdLine)
                                        .ThenBy(t => t.UnpackingStartDatetime);

            var system = from o in pageAndFiltered
                         select new LgwLupLotUpPlanDto
                         {
                             Id = o.Id,
                             ProdLine = o.ProdLine,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             NoInShift = o.NoInShift,
                             NoInDay = o.NoInDay,
                             LotNo = o.LotNo,
                             LotPartialNo = o.LotPartialNo,
                             UnpackingStartDatetime = o.UnpackingStartDatetime,
                             UnpackingFinishDatetime = o.UnpackingFinishDatetime,
                             Tpm = o.Tpm,
                             Remarks = o.Remarks,
                             UpCalltime = o.UpCalltime,
                             UnpackingActualFinishDatetime = o.UnpackingActualFinishDatetime,
                             UnpackingActualStartDatetime =o.UnpackingActualStartDatetime,
                             UpStatus = o.UpStatus,
                             MakingFinishDatetime = o.MakingFinishDatetime,
                             MkStatus =o.MkStatus,
                             IsActive = o.IsActive
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwLupLotUpPlanDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetLotUpPlanToExcel(GetLgwLupLotUpPlanExportInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll().WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime).WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate);

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate)
                                       .ThenBy(s => s.ProdLine)
                                       .ThenBy(t => t.UnpackingStartDatetime);

            var query = from o in pageAndFiltered
                        select new LgwLupLotUpPlanDto
                        {
                            Id = o.Id,
                            ProdLine = o.ProdLine,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            NoInShift = o.NoInShift,
                            NoInDay = o.NoInDay,
                            LotNo = o.LotNo,
                            LotPartialNo = o.LotPartialNo,
                            UnpackingStartDatetime = o.UnpackingStartDatetime,
                            UnpackingFinishDatetime = o.UnpackingFinishDatetime,
                            Tpm = o.Tpm,
                            Remarks = o.Remarks,
                            UpCalltime = o.UpCalltime,
                            UnpackingActualFinishDatetime = o.UnpackingActualFinishDatetime,
                            UnpackingActualStartDatetime = o.UnpackingActualStartDatetime,
                            UpStatus = o.UpStatus,
                            MakingFinishDatetime = o.MakingFinishDatetime,
                            MkStatus = o.MkStatus,
                            IsActive = o.IsActive
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
         
        //Import Data From Excel
        public async Task<List<ImportLotUpPlanDto>> ImportPxPUpPlanFromExcel(List<ImportLotUpPlanDto> LotUpPlans)
        {
            try
            {
                List<LgwLupLotUpPlan_T> LotUpPlan = new List<LgwLupLotUpPlan_T> { };
                foreach (var item in LotUpPlans)
                {
                    LgwLupLotUpPlan_T importData = new LgwLupLotUpPlan_T();
                    {
                        importData.Guid = item.Guid;
                        importData.WorkingDate = item.WorkingDate;
                        importData.ProdLine = item.ProdLine;
                        importData.LotNo = item.LotNo;
                        importData.Shift = item.Shift;
                        importData.UnpackingStartDatetime = item.UnpackingStartDatetime;
                        importData.UnpackingFinishDatetime = item.UnpackingFinishDatetime;
                        importData.Tpm = item.Tpm;
                        importData.Remarks = item.Remarks;
                    }
                    LotUpPlan.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(LotUpPlan);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To PxPUpPlan
        public async Task MergeDataLotUpPlan(string v_Guid)
        {
            string _sql = "Exec LGW_LOT_UP_PLAN_MERGE @Guid";
            await _dapperRepo.QueryAsync<LgwLupLotUpPlan>(_sql, new { Guid = v_Guid });
        }

        //get Data Screen


    }
}
