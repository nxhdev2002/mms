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
using prod.LogA.Lds.Dto;
using prod.LogA.Lds.Exporting;
using prod.LogA.Lds.LotPlan.ImportDto;

namespace prod.LogA.Lds
{
    //  [AbpAuthorize(AppPermissions.Pages_LogA_Lds_LotPlan)]
    public class LgaLdsLotPlanAppService : prodAppServiceBase, ILgaLdsLotPlanAppService
    {
        private readonly IDapperRepository<LgaLdsLotPlan, long> _dapperRepo;
        private readonly IRepository<LgaLdsLotPlan, long> _repo;
        private readonly ILgaLdsLotPlanExcelExporter _calendarListExcelExporter;

        public LgaLdsLotPlanAppService(IRepository<LgaLdsLotPlan, long> repo,
                                         IDapperRepository<LgaLdsLotPlan, long> dapperRepo,
                                        ILgaLdsLotPlanExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Lds_LotPlan_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgaLdsLotPlanDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgaLdsLotPlanDto input)
        {
            var mainObj = ObjectMapper.Map<LgaLdsLotPlan>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgaLdsLotPlanDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Lds_LotPlan_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgaLdsLotPlanDto>> GetAll(GetLgaLdsLotPlanInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var userId = AbpSession.UserId;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(s => s.ProdLine).ThenBy(t => t.Shift).ThenBy(a => a.SeqLineIn);

            var system = from o in pageAndFiltered

                         select new LgaLdsLotPlanDto
                         {
                             Id = o.Id,
                             ProdLine = o.ProdLine,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             SeqLineIn = o.SeqLineIn,
                             PlanStartDatetime = o.PlanStartDatetime,
                             Model = o.Model,
                             LotNo = o.LotNo,
                             LotNo2 = o.LotNo2,
                             Trip = o.Trip,
                             Dolly = o.Dolly,
                             StartDatetime = o.StartDatetime,
                             FinishDatetime = o.FinishDatetime,
                             DelaySecond = o.DelaySecond,
                             Status = o.Status,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();         
            var paged = system.PageBy(input);
            return new PagedResultDto<LgaLdsLotPlanDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetLotPlanToExcel(GetLgaLdsLotPlanExportInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var userId = AbpSession.UserId;

            var filtered = _repo.GetAll()
                 .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                 .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                 .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                 .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).OrderBy(s => s.ProdLine).ThenBy(t => t.Shift).ThenBy(a => a.SeqLineIn);

            var query = from o in pageAndFiltered
                        select new LgaLdsLotPlanDto
                        {
                            Id = o.Id,
                            ProdLine = o.ProdLine,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            SeqLineIn = (int)o.SeqLineIn,
                            PlanStartDatetime = o.PlanStartDatetime,
                            Model = o.Model,
                            LotNo = o.LotNo,
                            LotNo2 = o.LotNo2,
                            Trip = (int)o.Trip,
                            Dolly = o.Dolly,
                            StartDatetime = o.StartDatetime,
                            FinishDatetime = o.FinishDatetime,
                            DelaySecond =(int) o.DelaySecond,
                            Status = o.Status,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


        //Import Data From Excel
        public async Task<List<ImportLgaLdsLotPlanDto>> ImportLgaLdsLotPlanFromExcel(List<ImportLgaLdsLotPlanDto> LgaLdsLotPlans)
        {
            try
            {
                List<LgaLdsLotPlan_T> LgaLdsLotPlan = new List<LgaLdsLotPlan_T> { };
                foreach (var item in LgaLdsLotPlans)
                {
                    LgaLdsLotPlan_T importData = new LgaLdsLotPlan_T();
                    {
                        importData.Guid = item.Guid;
                        importData.WorkingDate = item.WorkingDate;
                        importData.ProdLine = item.ProdLine;
                        importData.Shift = item.Shift;
                        importData.SeqLineIn = item.SeqLineIn;
                        importData.PlanStartDatetime = item.PlanStartDatetime;
                        importData.LotNo = item.LotNo;
                        importData.LotNo2 = item.LotNo2;
                        importData.Trip = item.Trip;
                        importData.Dolly = item.Dolly;
                    }
                    LgaLdsLotPlan.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(LgaLdsLotPlan);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To PxPUpPlan
        public async Task MergeDataLgaLdsLotPlan(string v_Guid)
        { 
            string _sql = "Exec LGA_LDS_LOTPLAN_MERGE @Guid";
            await _dapperRepo.QueryAsync<LgaLdsLotPlan>(_sql, new { Guid = v_Guid });
        }
    }
}
