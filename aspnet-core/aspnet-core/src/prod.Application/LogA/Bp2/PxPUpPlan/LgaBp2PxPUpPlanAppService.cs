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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.LogA.Bp2.Dto;
using prod.LogA.Bp2.Exporting;
using prod.LogA.Bp2.PxPUpPlan.Dto;
using prod.LogA.Bp2.PxPUpPlan.Exporting;
using prod.LogA.Lds.Dto;
using prod.LogW.Pup.Dto;
using prod.Master.Common;
using prod.Master.Common.Dto;
using prod.Master.LogA.Dto;
using prod.Master.LogW;
using prod.Master.LogW.Dto;

namespace prod.LogA.Bp2.PxPUpPlan
{

    //  [AbpAuthorize(AppPermissions.Pages_LogA_Bp2_PxPUpPlan)]
    public class LgaBp2PxPUpPlanAppService : prodAppServiceBase, ILgaBp2PxPUpPlanAppService
    {
        private readonly IDapperRepository<LgaBp2PxPUpPlan, long> _dapperRepo;
        private readonly IRepository<LgaBp2PxPUpPlan, long> _repo;
        private readonly ILgaBp2PxPUpPlanExcelExporter _calendarListExcelExporter;
        private readonly IDapperRepository<MstLgwScreenConfig, long> _dapperRepoScreenConfig;
        private readonly IDapperRepository<MstCmmLookup, long> _dapperRepoLookUp;

        public LgaBp2PxPUpPlanAppService(IRepository<LgaBp2PxPUpPlan, long> repo,
                                         IDapperRepository<LgaBp2PxPUpPlan, long> dapperRepo,
                                        ILgaBp2PxPUpPlanExcelExporter calendarListExcelExporter,
                                        IDapperRepository<MstLgwScreenConfig, long> dapperRepoScreenConfig,
                                        IDapperRepository<MstCmmLookup, long> dapperRepoLookUp)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _dapperRepoScreenConfig = dapperRepoScreenConfig;
            _dapperRepoLookUp = dapperRepoLookUp;
        }

        public async Task<PagedResultDto<LgaBp2PxPUpPlanDto>> GetAll(GetLgaBp2PxPUpPlanInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(s => s.ProdLine).ThenBy(t => t.NoOfALineIn);

            var system = from o in pageAndFiltered
                         select new LgaBp2PxPUpPlanDto
                         {
                             Id = o.Id,
                             ProdLine = o.ProdLine,
                             NoOfALineIn = o.NoOfALineIn,
                             UnpackingTime = o.UnpackingTime,
                             UnpackingDate = o.UnpackingDate,
                             CaseNo = o.CaseNo,
                             SupplierNo = o.SupplierNo,
                             Model = o.Model,
                             TotalNoInShift = o.TotalNoInShift,
                             UnpackingDatetime = o.UnpackingDatetime,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             UpTable = o.UpTable,
                             UpLt = o.UpLt,
                             UnpackingStartDatetime = o.UnpackingStartDatetime,
                             UnpackingFinishDatetime = o.UnpackingFinishDatetime,
                             UnpackingSecond = o.UnpackingSecond,
                             UnpackingBy = o.UnpackingBy,
                             DelaySecond = o.DelaySecond,
                             TimeOffSecond = o.TimeOffSecond,
                             StartPauseTime = o.StartPauseTime,
                             EndPauseTime = o.EndPauseTime,
                             DelayConfirmFlag = o.DelayConfirmFlag,
                             TimeOffConfirmSecond = o.TimeOffConfirmSecond,
                             WhLocation = o.WhLocation,
                             InvoiceDate = o.InvoiceDate,
                             Remarks = o.Remarks,
                             IsNewPart = o.IsNewPart,
                             IsActive = o.IsActive,
                             Status = o.Status
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgaBp2PxPUpPlanDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetPxPUpPlanToExcel(GetLgaBp2PxPUpPlanExportInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(s => s.ProdLine).ThenBy(t => t.NoOfALineIn);

            var query = from o in pageAndFiltered
                        select new LgaBp2PxPUpPlanDto
                        {
                            Id = o.Id,
                            ProdLine = o.ProdLine,
                            NoOfALineIn = o.NoOfALineIn,
                            UnpackingTime = o.UnpackingTime,
                            UnpackingDate = o.UnpackingDate,
                            CaseNo = o.CaseNo,
                            SupplierNo = o.SupplierNo,
                            Model = o.Model,
                            TotalNoInShift = o.TotalNoInShift,
                            UnpackingDatetime = o.UnpackingDatetime,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            UpTable = o.UpTable,
                            UpLt = o.UpLt,
                            UnpackingStartDatetime = o.UnpackingStartDatetime,
                            UnpackingFinishDatetime = o.UnpackingFinishDatetime,
                            UnpackingSecond = o.UnpackingSecond,
                            UnpackingBy = o.UnpackingBy,
                            DelaySecond = o.DelaySecond,
                            TimeOffSecond = o.TimeOffSecond,
                            StartPauseTime = o.StartPauseTime,
                            EndPauseTime = o.EndPauseTime,
                            DelayConfirmFlag = o.DelayConfirmFlag,
                            TimeOffConfirmSecond = o.TimeOffConfirmSecond,
                            WhLocation = o.WhLocation,
                            InvoiceDate = o.InvoiceDate,
                            Remarks = o.Remarks,
                            IsNewPart = o.IsNewPart,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        //
        #region Big Part 2x2


        public async Task<PagedResultDto<MstLgaModuleUpTableDto>> GetDataModuleUpTable(string Line)
        {
            string _sql3 = "Exec MST_LGA_MODULE_UP_TABLE @Line";

            var data3 = await _dapperRepoLookUp.QueryAsync<MstLgaModuleUpTableDto>(_sql3, new { @Line = Line });
            var rsModuleUPTable = from o in data3
                                  select new MstLgaModuleUpTableDto
                                  {
                                      Id = o.Id,
                                      Line = o.Line,
                                      UpTable = o.UpTable,
                                      DisplayOrder = o.DisplayOrder,
                                      IsActive = o.IsActive,
                                  };

            var totalCount = data3.ToList().Count;

            return new PagedResultDto<MstLgaModuleUpTableDto>(
                totalCount,
                rsModuleUPTable.ToList()
            );
        }

        public async Task<List<object>> GetDataBigPart2X2Screen(string id,string DomainCode,string ProdLine, string ProLine_UpCase)
            {
                List<object> list = new List<object>();
 //Screen config
                string _sql1 = "Exec MST_LGW_SCREEN_CONFIG_GET @id";
                var data1 = await Task.Run(() => _dapperRepoScreenConfig.QueryAsync<MstLgwScreenConfigDto>(_sql1, new { @id = id }));
                var rsScreenConfig = from o in data1
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
//Lookup list by domain code 
            string _sql2 = "Exec MST_CMN_LOOKUP_LIST_BY_DOMAIN_CODE @DomainCode";
            var data2 = await _dapperRepoLookUp.QueryAsync<MstCmmLookupDto>(_sql2, new { @DomainCode = DomainCode });
            var rsLookUpbyDomain = from o in data2
                          select new MstCmmLookupDto
                          {
                              Id = o.Id,
                              DomainCode = o.DomainCode,
                              ItemCode = o.ItemCode,
                              ItemValue = o.ItemValue,
                              ItemOrder = o.ItemOrder,
                              Description = o.Description,
                              IsUse = o.IsUse,
                              IsRestrict = o.IsRestrict,
                          };

//Module UP Table 
            //string _sql3 = "Exec MST_LGA_MODULE_UP_TABLE @Line";

            //var data3 = await _dapperRepoLookUp.QueryAsync<MstLgaModuleUpTableDto>(_sql3, new { @Line = Line });
            //var rsModuleUPTable = from o in data3
            //              select new MstLgaModuleUpTableDto
            //              {
            //                  Id = o.Id,
            //                  Line = o.Line,
            //                  UpTable = o.UpTable,
            //                  DisplayOrder = o.DisplayOrder,
            //                  IsActive = o.IsActive,
            //              };


//Up Plan Base Today
            string _sql4 = "Exec LGA_BP2_PXP_UP_PLAN_BASE_TODAY @ProdLine";
            var data4 = await _dapperRepoLookUp.QueryAsync<LgaBp2PxpUpPlanBaseDto>(_sql4, new { @ProdLine = ProdLine });
            var rsUpPlanBaseToday = from o in data4
                          select new LgaBp2PxpUpPlanBaseDto
                          {
                              Id = o.Id,
                              WorkingDate = o.WorkingDate,
                              ProdLine = o.ProdLine,
                              Shift = o.Shift,
                              StartNoInShift = o.StartNoInShift,
                              EndNoInShift = o.EndNoInShift,
                              IsActive = o.IsActive,
                          };

//Up Plan Up Case
            string _sql5 = "Exec LGA_BP2_PXP_UP_PLAN_UP_CASE_GETS @ProdLine";
            var data5 = await _dapperRepo.QueryAsync<LgaBp2PxpUpPlanUpCaseDto>(_sql5, new { @ProdLine = ProLine_UpCase });
            var rsUpPlanUpCase = from o in data5
                          select new LgaBp2PxpUpPlanUpCaseDto
                          {
                              Id = o.Id,
                              ProdLine = o.ProdLine,
                              NoOfALineIn = o.NoOfALineIn,
                              CaseNo = o.CaseNo,
                              SupplierNo = o.SupplierNo,
                              WorkingDate = o.WorkingDate,
                              Shift = o.Shift,
                              UpTable = o.UpTable,
                              UpLt = o.UpLt,                         
                              UnpackingTime = o.UnpackingTime,
                              UnpackingDatetime = o.UnpackingDatetime,
                              CycleTime = o.CycleTime,
                              CycleSecond = o.CycleSecond,
                              TaktTime = o.TaktTime,
                              TaktTimeSecond = o.TaktTimeSecond,
                              TackTime = o.TackTime,
                              WhLocation = o.WhLocation,
                              IsNewPart = o.IsNewPart,
                              IsActive = o.IsActive
                          };

//Get Last Scan
            string _sql6 = "Exec LGA_BP2_A_QVG_GET_LAST_SCAN";

            var data6 = await _dapperRepo.QueryAsync<LgaBp2AQvgGetLastScanDto>(_sql6, new { });
            var rsLastScan = from o in data6
                          select new LgaBp2AQvgGetLastScanDto
                          {
                              Id = o.Id,
                              ScanTime = o.ScanTime,
                              ProdLine = o.ProdLine,
                              SequenceNo = o.SequenceNo,
                              NoInDate = o.NoInDate,
                              WorkingDate = o.WorkingDate,
                              IsManual = o.IsManual,
                              AdjNo = o.AdjNo,
                              DelayConfirmSecond = o.DelayConfirmSecond,
                              CreateDate = o.CreateDate
                          };

//Get Up Plan Working
            string _sql7 = "Exec LGA_BP2_PXP_UP_PLAN_GET_WORKING_DATA";

            var data7 = await _dapperRepo.QueryAsync<LgaBp2PxPUpPlanDto>(_sql7, new { });
            var rsUpPlanWorkingData = from o in data7
                          select new LgaBp2PxPUpPlanDto
                          {
                              Id = o.Id,
                              ProdLine = o.ProdLine,
                              NoOfALineIn = o.NoOfALineIn,
                              UnpackingTime = o.UnpackingTime,
                              UnpackingDate = o.UnpackingDate,
                              CaseNo = o.CaseNo,
                              SupplierNo = o.SupplierNo,
                              WorkingDate = o.WorkingDate,
                              Shift = o.Shift,
                              UpTable = o.UpTable,
                              UpLt = o.UpLt,
                              IsNewPart = o.IsNewPart,
                              Status = o.Status,
                              UnpackingStartDatetime = o.UnpackingStartDatetime,
                              UnpackingFinishDatetime = o.UnpackingFinishDatetime,
                              UnpackingSecond = o.UnpackingSecond,
                              DelaySecond = o.DelaySecond,
                              TimeOffSecond = o.TimeOffSecond,
                              DelayConfirmFlag = o.DelayConfirmFlag,
                              ScreenStatus = o.ScreenStatus

                          };
//Get Up Plan UpCase Delay
            string _sql8 = "Exec LGA_BP2_PXP_UP_PLAN_GETS_UPCASE_DELAY @ProdLine";

            var data8 = await _dapperRepo.QueryAsync<LgaBp2PxpUpPlanUpCaseDto>(_sql8, new { @ProdLine = ProLine_UpCase });
            var rsUpCaseDelay = from o in data8
                          select new LgaBp2PxpUpPlanUpCaseDto
                          {
                              Id = o.Id,
                              ProdLine = o.ProdLine,
                              NoOfALineIn = o.NoOfALineIn,
                              CaseNo = o.CaseNo,
                              SupplierNo = o.SupplierNo,
                              WorkingDate = o.WorkingDate,
                              Shift = o.Shift,
                              UpTable = o.UpTable,
                              UpLt = o.UpLt,
                              IsNewPart = o.IsNewPart,
                              IsActive = o.IsActive,
                              UnpackingTime = o.UnpackingTime,
                              UnpackingDatetime = o.UnpackingDatetime,
                              CycleTime = o.CycleTime,
                              CycleSecond = o.CycleSecond,
                              TaktTime = o.TaktTime,
                              TaktTimeSecond = o.TaktTimeSecond,
                              TackTime = o.TackTime,
                              MaxNoInDate = o.MaxNoInDate,
                              WhLocation = o.WhLocation,
                              IsCurrentDate = o.IsCurrentDate,
                              MaxNoOfALineIn =  o.MaxNoOfALineIn
                          };

            //Add obj to list
            list.Add(rsScreenConfig);
            list.Add(rsLookUpbyDomain);
           // list.Add(rsModuleUPTable);
            list.Add(rsUpPlanBaseToday);
            list.Add(rsUpPlanUpCase);
            list.Add(rsLastScan);
            list.Add(rsUpPlanWorkingData);
            list.Add(rsUpCaseDelay);
            return list;
        }

        #endregion

    }
}

