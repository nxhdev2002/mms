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
using NPOI.SS.Formula.Functions;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inv.D125.Dto;
using prod.Inv.Proc;
using prod.Inv.Proc.Dto;
using prod.Inv.Proc.Exporting;
using prod.LogA.Bp2.Dto;
using prod.HistoricalData;

namespace prod.Inv.Proc
{

    //  [AbpAuthorize(AppPermissions.Pages_Inv_Proc_ProductionMapping)]
    public class InvProductionMappingAppService : prodAppServiceBase, IInvProductionMappingAppService
    {
        private readonly IDapperRepository<InvProductionMapping, long> _dapperRepo;
        private readonly IRepository<InvProductionMapping, long> _repo;
        private readonly IInvProductionMappingExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public InvProductionMappingAppService(IRepository<InvProductionMapping, long> repo,
                                         IDapperRepository<InvProductionMapping, long> dapperRepo,
                                        IInvProductionMappingExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetProductionMappingHistory(GetInvProductionMappingHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvProductionMappingHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("InvProductionMapping");
        }
        public async Task<PagedResultDto<InvProductionMappingDto>> GetAll(InvProductionMappingInput input)
        {
            string _sql = "Exec INV_PROC_PRODUCTION_MAPPING_GETDATA @PeriodId, @Shop, @BodyNo, @LotNo, @Date_in_from, @Date_in_to";

            var filtered = await _dapperRepo.QueryAsync<InvProductionMappingDto>(_sql, 
                new {
                    @PeriodId = input.PeriodId,
                    @Shop = input.Shop,
                    @BodyNo = input.BodyNo, 
                    @LotNo = input.LotNo, 
                    @Date_in_from = input.DateInFrom,
                    @Date_in_to = input.DateInTo
                });
            var results = from o in filtered
                          select new InvProductionMappingDto
                          {
                              PlanSequence = o.PlanSequence,
                              Shop = o.Shop,
                              Model = o.Model,
                              LotNo = o.LotNo,
                              NoInLot = o.NoInLot,
                              Grade = o.Grade,
                              BodyNo = o.BodyNo,
                              DateIn = o.DateIn,
                              TimeIn = o.TimeIn,
                              UseLotNo = o.UseLotNo,
                              SupplierNo = o.SupplierNo,
                              PartId = o.PartId,
                              Quantity = o.Quantity,
                              Cost = o.Cost,
                              Cif = o.Cif,
                              Fob = o.Fob,
                              Freight = o.Freight,
                              Insurance = o.Insurance,
                              Thc = o.Thc,
                              Tax = o.Tax,
                              InLand = o.InLand,
                              Amount = o.Amount,
                              PeriodId = o.PeriodId,
                              CostVn = o.CostVn,
                              CifVn = o.CifVn,
                              FobVn = o.FobVn,
                              FreightVn = o.FreightVn,
                              InsuranceVn = o.InsuranceVn,
                              ThcVn = o.ThcVn,
                              TaxVn = o.TaxVn,
                              InlandVn = o.InlandVn,
                              AmountVn = o.AmountVn,
                              WipId = o.WipId,
                              InStockId = o.InStockId,
                              MappingId = o.MappingId,
                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<InvProductionMappingDto>(
                totalCount,
                results.ToList()
            );
        }
        public async Task<PagedResultDto<InvPeriodDto>> GetIdInvPeriod()
        {
            string _sql = "Exec  INV_PERIOD_GETID";

            var data = await _dapperRepo.QueryAsync<InvPeriodDto>(_sql, new { });

            var results = from d in data
                          select new InvPeriodDto
                          {
                              Id = d.Id,
                              Description = d.Description,
                              From_Date = d.From_Date,
                              To_Date = d.To_Date,
                              Status = d.Status
                          };

            var totalCount = data.ToList().Count;
            return new PagedResultDto<InvPeriodDto>(
                totalCount,
                results.ToList()
            );
        }


        public async Task<FileDto> GetExportToFile(InvProductionMappingInput input)
        {
            string _sql = "Exec INV_PROC_PRODUCTION_MAPPING_GETDATA @PeriodId, @Shop, @BodyNo, @LotNo, @Date_in_from, @Date_in_to";

            var filtered = await _dapperRepo.QueryAsync<InvProductionMappingDto>(_sql,
                new
                {
                    @PeriodId = input.PeriodId,
                    @Shop = input.Shop,
                    @BodyNo = input.BodyNo,
                    @LotNo = input.LotNo,
                    @Date_in_from = input.DateInFrom,
                    @Date_in_to = input.DateInTo
                });
            var results = from o in filtered
                          select new InvProductionMappingDto
                          {
                              PlanSequence = o.PlanSequence,
                              Shop = o.Shop,
                              Model = o.Model,
                              LotNo = o.LotNo,
                              NoInLot = o.NoInLot,
                              Grade = o.Grade,
                              BodyNo = o.BodyNo,
                              DateIn = o.DateIn,
                              TimeIn = o.TimeIn,
                              UseLotNo = o.UseLotNo,
                              SupplierNo = o.SupplierNo,
                              PartId = o.PartId,
                              Quantity = o.Quantity,
                              Cost = o.Cost,
                              Cif = o.Cif,
                              Fob = o.Fob,
                              Freight = o.Freight,
                              Insurance = o.Insurance,
                              Thc = o.Thc,
                              Tax = o.Tax,
                              InLand = o.InLand,
                              Amount = o.Amount,
                              PeriodId = o.PeriodId,
                              CostVn = o.CostVn,
                              CifVn = o.CifVn,
                              FobVn = o.FobVn,
                              FreightVn = o.FreightVn,
                              InsuranceVn = o.InsuranceVn,
                              ThcVn = o.ThcVn,
                              TaxVn = o.TaxVn,
                              InlandVn = o.InlandVn,
                              AmountVn = o.AmountVn,
                              WipId = o.WipId,
                              InStockId = o.InStockId,
                              MappingId = o.MappingId,
                          };

            var exportToExcel = results.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
             
    }

}
