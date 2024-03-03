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
using prod.Frame.Andon.Dto;
using prod.LogA.Lds;
using prod.LogA.Lds.LotPlan.ImportDto;
using prod.LogW.Lup;
using prod.LogW.Lup.Dto;
using prod.Master.WorkingPattern.Dto;
using prod.Welding.Andon.Dto;
using prod.Welding.Andon.Exporting;
using prod.Welding.Andon.ImportDto;
using prod.Assy.Andon.Dto;
using prod.HistoricalData;

namespace prod.Welding.Andon
{
      [AbpAuthorize(AppPermissions.Pages_ProdPlan_WeldingPlan_View)]
    public class WldAdoWeldingPlanAppService : prodAppServiceBase, IWldAdoWeldingPlanAppService
    {
        private readonly IDapperRepository<WldAdoWeldingPlan, long> _dapperRepo;
        private readonly IRepository<WldAdoWeldingPlan, long> _repo;
        private readonly IWldAdoWeldingPlanExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public WldAdoWeldingPlanAppService(IRepository<WldAdoWeldingPlan, long> repo,
                                         IDapperRepository<WldAdoWeldingPlan, long> dapperRepo,
                                        IWldAdoWeldingPlanExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetWldAdoWeldingPlanHistory(GetWldAdoWeldingPlanHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetWldAdoWeldingPlanHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            var data = await _historicalDataAppService.GetChangedRecordIds("WldAdoWeldingPlan");
            return data;
        }

        [AbpAuthorize(AppPermissions.Pages_ProdPlan_WeldingPlan_Edit)]
        public async Task CreateOrEdit(CreateOrEditWldAdoWeldingPlanDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditWldAdoWeldingPlanDto input)
        {
            var mainObj = ObjectMapper.Map<WldAdoWeldingPlan>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditWldAdoWeldingPlanDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

          [AbpAuthorize(AppPermissions.Pages_ProdPlan_WeldingPlan_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<WldAdoWeldingPlanDto>> GetAll(GetWldAdoWeldingPlanInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var v_WorkingDate = dateTime.ToString("yyyy") + "-" + dateTime.ToString("MM") + "-" + dateTime.ToString("dd");

            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.VinNo), e => e.VinNo.Contains(input.VinNo))
                .WhereIf(!input.RequestDateFrom.HasValue, t => t.RequestDate >= dateTime)
                .WhereIf(input.RequestDateFrom.HasValue, t => t.RequestDate >= input.RequestDateFrom.Value)
                .WhereIf(input.RequestDateTo.HasValue, t => t.RequestDate <= input.RequestDateTo.Value)
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.RequestDate).ThenBy(s => s.PlanTime);

            var results = from o in pageAndFiltered
                         select new WldAdoWeldingPlanDto
                         {
                             Id = o.Id,
                             Model = o.Model,
                             LotNo = o.LotNo,
                             NoInLot = o.NoInLot,
                             Grade = o.Grade,
                             ProdLine = o.ProdLine,
                             BodyNo = o.BodyNo,
                             VinNo = o.VinNo,
                             Color = o.Color,
                             PlanTime = o.PlanTime,
                             RequestDate = o.RequestDate,
                             Shift = o.Shift,
                             WInDate = o.WInDate,
                             WOutDate = o.WOutDate,
                             EdIn = o.EdIn,
                             TInPlanDatetime = o.TInPlanDatetime,
                             VehicleId = o.VehicleId,
                             Cfc = o.Cfc,
                             WeldingId = o.WeldingId,
                             AssemblyId = o.AssemblyId
                         };

            var totalCount = (await filtered.ToListAsync()).Count;
            var paged = results.AsQueryable().PageBy(input);

            return new PagedResultDto<WldAdoWeldingPlanDto>(
                totalCount,
                await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetWeldingPlanToExcel(GetWldAdoWeldingPlanExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var v_WorkingDate = dateTime.ToString("yyyy") + "-" + dateTime.ToString("MM") + "-" + dateTime.ToString("dd");

            var filtered = _repo.GetAll()
              .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
              .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
              .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.VinNo), e => e.VinNo.Contains(input.VinNo))
              .WhereIf(!input.RequestDateFrom.HasValue, t => t.RequestDate >= dateTime)
              .WhereIf(input.RequestDateFrom.HasValue, t => t.RequestDate >= input.RequestDateFrom.Value)
              .WhereIf(input.RequestDateTo.HasValue, t => t.RequestDate <= input.RequestDateTo.Value)
              ;
            var pageAndFiltered = filtered.OrderBy(s => s.RequestDate).ThenBy(s => s.PlanTime);

            var query = from o in pageAndFiltered
                        select new WldAdoWeldingPlanDto
                        {
                            Id = o.Id,
                            Model = o.Model,
                            LotNo = o.LotNo,
                            NoInLot = o.NoInLot,
                            Grade = o.Grade,
                            ProdLine = o.ProdLine,
                            BodyNo = o.BodyNo,
                            VinNo = o.VinNo,
                            Color = o.Color,
                            PlanTime = o.PlanTime,
                            RequestDate = o.RequestDate,
                            Shift = o.Shift,
                            WInDate = o.WInDate,
                            WOutDate = o.WOutDate,
                            EdIn = o.EdIn,
                            TInPlanDatetime = o.TInPlanDatetime,
                            VehicleId = o.VehicleId,
                            Cfc = o.Cfc,
                            WeldingId = o.WeldingId,
                            AssemblyId = o.AssemblyId
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        //Import Data From Excel
        [AbpAuthorize(AppPermissions.Pages_ProdPlan_WeldingPlan_Import)]
        public async Task<List<ImportWldAdoWeldingPlanDto>> ImportWldAdoWeldingPlanFromExcel(List<ImportWldAdoWeldingPlanDto> WeldingPlans)
        {
            try
            {
                List<WldAdoWeldingPlan_T> WeldingPlan = new List<WldAdoWeldingPlan_T> { };
                foreach (var item in WeldingPlans)
                {
                    WldAdoWeldingPlan_T importData = new WldAdoWeldingPlan_T();
                    {
                        importData.Guid = item.Guid;
                        importData.Shift = item.Shift;
                        importData.Model = item.Model;
                        importData.LotNo = item.LotNo;
                        importData.NoInLot = item.NoInLot;
                        importData.Grade = item.Grade;
                        importData.BodyNo = item.BodyNo;
                        importData.VinNo = item.VinNo;
                        importData.PlanTime = item.PlanTime;
                        importData.RequestDate = item.RequestDate;
                        importData.Welding = item.Welding;
                    }
                    WeldingPlan.Add(importData);
                }
                await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(WeldingPlan);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }


        //Import Data Ed In From Excel
        public async Task<List<ImportWldAdoEdInDto>> ImportWldAdoEdInFromExcel(List<ImportWldAdoEdInDto> EdIns)
        {
            try
            {
                List<WldAdoEdIn_T> EdIn = new List<WldAdoEdIn_T> { };
                foreach (var item in EdIns)
                {
                    WldAdoEdIn_T importDataEd = new WldAdoEdIn_T();
                    {
                        importDataEd.Guid = item.Guid;
                        importDataEd.Shift = item.Shift;
                        importDataEd.Model = item.Model;
                        importDataEd.LotNo = item.LotNo;
                        importDataEd.NoInLot = item.NoInLot;
                        importDataEd.BodyNo = item.BodyNo;
                        importDataEd.Vin = item.Vin;
                        importDataEd.PlanTime = item.PlanTime;
                        importDataEd.RequestDate = item.RequestDate;
                        importDataEd.ProdLine = item.ProdLine;
                    }
                    EdIn.Add(importDataEd);
                }
                await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(EdIn);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To WeldingPlan
        public async Task MergeDataWeldingPlan(string v_Guid)
        {
            string _sql = "Exec WLD_ADO_WELDINGPLAN_MERGE @Guid";
            await _dapperRepo.QueryAsync<ImportWldAdoWeldingPlanDto>(_sql, new { Guid = v_Guid });
        }

        public async Task UpdateUserId(string _guid)
        {
            string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                Guid = _guid,
                p_UserId = AbpSession.UserId
            });
        }

        //get messError Import
        public async Task<PagedResultDto<WldAdoWeldingPlanDto>> GetMessageErrorWeldingPlanImport(string v_Guid)
        {
            var data = await _dapperRepo.QueryAsync<WldAdoWeldingPlanDto>("Exec WLD_ADO_WELDING_PLAN_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });
            var rsData = from o in data
                         select new WldAdoWeldingPlanDto
                         {
                             Id = o.Id,
                             ProdLine = o.ProdLine,
                             Model = o.Model,
                             LotNo = o.LotNo,
                             NoInLot = o.NoInLot,
                             Grade = o.Grade,
                             BodyNo = o.BodyNo,
                             VinNo = o.VinNo,
                             PlanTime = o.PlanTime,
                             RequestDate = o.RequestDate,
                             Shift = o.Shift,
                             MessagesError = o.MessagesError
                         };

            var totalCount = rsData.Count();
            return new PagedResultDto<WldAdoWeldingPlanDto>(
                totalCount,
                 rsData.ToList()
            );
        }

        public async Task<FileDto> GetListErrWeldingPlanToExcel(string v_Guid)
        {
            var data = await _dapperRepo.QueryAsync<WldAdoWeldingPlanDto>("Exec WLD_ADO_WELDING_PLAN_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });
            var rsData = from o in data
                         select new WldAdoWeldingPlanDto
                         {
                             Id = o.Id,
                             ProdLine = o.ProdLine,
                             Model = o.Model,
                             LotNo = o.LotNo,
                             NoInLot = o.NoInLot,
                             Grade = o.Grade,
                             BodyNo = o.BodyNo,
                             VinNo = o.VinNo,
                             Color = o.Color,
                             PlanTime = o.PlanTime,
                             RequestDate = o.RequestDate,
                             Shift = o.Shift,
                             WInDate = o.WInDate,
                             WOutDate = o.WOutDate,
                             EdIn = o.EdIn,
                             MessagesError = o.MessagesError
                         };
            var exportToExcel = rsData.ToList();
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
        }
    }
}
