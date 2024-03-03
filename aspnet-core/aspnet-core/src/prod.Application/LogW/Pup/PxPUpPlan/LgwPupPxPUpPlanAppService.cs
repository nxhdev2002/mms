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
using DapperExtensions;
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
using prod.LogW.Pup.Dto;
using prod.LogW.Pup.Exporting;
using prod.LogW.Pup.ImportDto;

namespace prod.LogW.Pup
{
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Pup_PxPUpPlan)]
    public class LgwPupPxPUpPlanAppService : prodAppServiceBase, ILgwPupPxPUpPlanAppService
    {
        private readonly IDapperRepository<LgwPupPxPUpPlan, long> _dapperRepo;
        private readonly IRepository<LgwPupPxPUpPlan, long> _repo;
        private readonly ILgwPupPxPUpPlanExcelExporter _calendarListExcelExporter;

        public LgwPupPxPUpPlanAppService(IRepository<LgwPupPxPUpPlan, long> repo,
                                         IDapperRepository<LgwPupPxPUpPlan, long> dapperRepo,
                                        ILgwPupPxPUpPlanExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Pup_PxPUpPlan_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgwPupPxPUpPlanDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgwPupPxPUpPlanDto input)
        {
            var mainObj = ObjectMapper.Map<LgwPupPxPUpPlan>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgwPupPxPUpPlanDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Pup_PxPUpPlan_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        //Get All Data
        public async Task<PagedResultDto<LgwPupPxPUpPlanDto>> GetAll(GetLgwPupPxPUpPlanInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
           
            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))          
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate); ;

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(a => a.ProdLine).ThenBy(n => n.Shift).ThenBy(m => m.UnpackingStartDatetime);

            var system = from o in pageAndFiltered
                         select new LgwPupPxPUpPlanDto
                         {
                             Id = o.Id,
                             ProdLine = o.ProdLine,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             NoInShift = o.NoInShift,
                             SeqLineIn = o.SeqLineIn,
                             CaseNo = o.CaseNo,
                             SupplierNo = o.SupplierNo,
                             UpTable = o.UpTable,
                             IsNoPxpData = o.IsNoPxpData,
                             UnpackingStartDatetime = o.UnpackingStartDatetime,
                             UnpackingFinishDatetime = o.UnpackingFinishDatetime,
                             UnpackingTime = o.UnpackingTime,
                             UnpackingDate = o.UnpackingDate,
                             UnpackingDatetime = o.UnpackingDatetime,
                             UpLt = o.UpLt,
                             Status = o.Status,
                             DelaySecond = o.DelaySecond,
                             DelayConfirmFlag = o.DelayConfirmFlag,
                             WhLocation = o.WhLocation,
                             InvoiceDate = o.InvoiceDate,
                             Remarks = o.Remarks,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwPupPxPUpPlanDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        //Export Excel
        public async Task<FileDto> GetPxPUpPlanToExcel(GetLgwPupPxPUpPlanExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
           
            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))             
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate); ;

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(a => a.ProdLine).ThenBy(n => n.Shift).ThenBy(m => m.UnpackingStartDatetime);

            var query = from o in pageAndFiltered
                        select new LgwPupPxPUpPlanDto
                        {
                            Id = o.Id,
                            ProdLine = o.ProdLine,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            NoInShift = o.NoInShift,
                            SeqLineIn = o.SeqLineIn,
                            CaseNo = o.CaseNo,
                            SupplierNo = o.SupplierNo,
                            UpTable = o.UpTable,
                            IsNoPxpData = o.IsNoPxpData,
                            UnpackingStartDatetime = o.UnpackingStartDatetime,
                            UnpackingFinishDatetime = o.UnpackingFinishDatetime,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }



        //Import Data From PxPUpPlan Excel
        public async Task<List<ImportPxPUpPlanDto>> ImportPxPUpPlanFromExcel(List<ImportPxPUpPlanDto> PxpUpPlans)
        {
            try
            {
                List<LgwPupPxPUpPlan_T> PxPUpPlan = new List<LgwPupPxPUpPlan_T> { };
                foreach (var item in PxpUpPlans)
                {
                    LgwPupPxPUpPlan_T importData = new LgwPupPxPUpPlan_T();
                    {
                        importData.Guid = item.Guid;
                        importData.WorkingDate = item.WorkingDate;
                        importData.ProdLine = item.ProdLine;
                        importData.SeqLineIn = item.SeqLineIn;
                        importData.UnpackingStartDatetime = item.UnpackingStartDatetime;
                        importData.SupplierNo = item.SupplierNo;
                        importData.CaseNo = item.CaseNo;
                        importData.UpTable = item.UpTable;
                        importData.IsNoPxpData = item.IsNoPxpData;
                        importData.IsActive = "Y";
                    }
                    PxPUpPlan.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(PxPUpPlan);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To PxPUpPlan
        public async Task MergeDataPxPUpPlan(string v_Guid)
        {
            string _sql = "Exec LGW_PUP_PXP_UP_PLAN_MERGE @Guid";
            await _dapperRepo.QueryAsync<LgwPupPxPUpPlan>(_sql, new { Guid = v_Guid });
            
        }



        //get Data Screen
        public async Task<PagedResultDto<LgwPupPxPUpPlanOutputDto>> GetPxpUnpackPlan()
        {
            string _sql = "Exec LGW_PUP_PXP_UNPACKING_PLAN_GET_PROGRESS_DATA";

            var filtered = await _dapperRepo.QueryAsync<LgwPupPxPUpPlanOutputDto>(_sql, new { });
            var results = from d in filtered
                          select new LgwPupPxPUpPlanOutputDto
                          {
                              ProdLine = d.ProdLine,
                              WorkingDate = d.WorkingDate,
                              Shift = d.Shift,
                              NoInShift = d.NoInShift,
                              SeqLineIn = d.SeqLineIn,
                              CaseNo = d.CaseNo,
                              SupplierNo = d.SupplierNo,
                              UpTable = d.UpTable,
                              IsNoPxpData = d.IsNoPxpData,
                              UnpackingStartDatetime = d.UnpackingStartDatetime,
                              UnpackingDate = d.UnpackingDate,
                              UnpackingDatetime = d.UnpackingDatetime,
                              UnpackingTime = d.UnpackingTime,
                              UpLt = d.UpLt,
                              DelayConfirmFlag = d.DelayConfirmFlag,
                              DelaySecond = d.DelaySecond,
                              Status = d.Status,
                              WhLocation = d.WhLocation,
                              IsActive = d.IsActive,
                              IsFinished = d.IsFinished,
                              IsStarted = d.IsStarted,
                              IsDelayed = d.IsDelayed,
                              IsCalling = d.IsCalling,
                              IsPreviousShiftDelay = d.IsPreviousShiftDelay,
                              TotalBlock = d.TotalBlock,
                              PlanVol = d.PlanVol,
                              ActualVol = d.ActualVol,
                              LeadTime = d.LeadTime,
                              TaktTime = d.TaktTime
                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<LgwPupPxPUpPlanOutputDto>(
                totalCount,
                results.ToList()
            );
        }
    }
}
