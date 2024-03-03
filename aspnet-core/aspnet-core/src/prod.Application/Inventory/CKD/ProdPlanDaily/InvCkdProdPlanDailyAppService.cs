using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using GemBox.Spreadsheet;
using prod.Authorization;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_ProdPlan_ProdPlanDaily_View)]
    public class InvCkdProdPlanDailyAppService : prodAppServiceBase, IInvCkdProdPlanDailyAppService
    {
        private readonly IDapperRepository<InvCkdProdPlanDaily, long> _dapperRepo;
        private readonly IRepository<InvCkdProdPlanDaily, long> _repo;
        private readonly IInvCkdProdPlanDailyExcelExporter _prodPlanDailyExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IHistoricalDataAppService _historicalDataAppService;


        public InvCkdProdPlanDailyAppService(IRepository<InvCkdProdPlanDaily, long> repo,
                                             IDapperRepository<InvCkdProdPlanDaily, long> dapperRepo,
                                            IInvCkdProdPlanDailyExcelExporter prodPlanDailyExcelExporter,
                                            ITempFileCacheManager tempFileCacheManager,
                                            IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _prodPlanDailyExcelExporter = prodPlanDailyExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetProdPlanDailyHistory(GetInvCkdProdPlanDailyHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvCkdProdPlanDailyHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _prodPlanDailyExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            /*            var data = await _historicalDataAppService.GetChangedRecordIds("InvCkdProdPlanDaily");
                        return data;*/
            return await _historicalDataAppService.GetChangedRecordIds("InvCkdProdPlanDaily");
        } 

        public async Task<PagedResultDto<InvCkdProdPlanDailyDto>> GetAll(GetInvCkdProdPlanDailyInput input)
        {
            string _sql = "Exec INV_CKD_PROD_PLAN_DAILY_GETS @p_Vin, @p_LotNo, @p_NoInLot, @p_Cfc, @p_BodyNo, " +
                "@p_DateFrom, @p_DateTo, @p_IsPdiDate, @p_SelectDate";

            IEnumerable<InvCkdProdPlanDailyDto> result = await _dapperRepo.QueryAsync<InvCkdProdPlanDailyDto>(_sql, new
            {
                p_Vin = input.Vin,
                p_LotNo = input.LotNo,
                p_NoInLot = input.NoInLot,
                p_Cfc = input.Cfc,
                p_BodyNo = input.BodyNo,
                p_DateFrom = input.DateFrom,
                p_DateTo = input.DateTo,
                p_IsPdiDate = input.IsPdiDate,
                p_SelectDate = input.SelectDate
            });

            var listResult = result.ToList();
            if (listResult.Count > 0 && input.DateFrom != null && input.DateTo != null)
            {
                listResult[0].CountWinPlan = listResult.Where(x => x.WInPlanDate >= input.DateFrom.Value.Date && x.WInPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountWoutPlan = listResult.Where(x => x.WOutPlanDate >= input.DateFrom.Value.Date && x.WOutPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountTinPlan = listResult.Where(x => x.TInPlanDate >= input.DateFrom.Value.Date && x.TInPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountToutPlan = listResult.Where(x => x.TOutPlanDate >= input.DateFrom.Value.Date && x.TOutPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountAinPlan = listResult.Where(x => x.AInPlanDate >= input.DateFrom.Value.Date && x.AInPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountAoutPlan = listResult.Where(x => x.AOutPlanDate >= input.DateFrom.Value.Date && x.AOutPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountPdiDate = listResult.Where(x => x.PdiDate >= input.DateFrom.Value.Date && x.PdiDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountLineoffdate = listResult.Where(x => x.LineoffDate >= input.DateFrom.Value.Date && x.LineoffDate < input.DateTo.Value.Date.AddDays(1)).Count();
            }
            else if(listResult.Count > 0 && input.DateFrom != null && input.DateTo == null)
            {
                listResult[0].CountWinPlan = listResult.Where(x => x.WInPlanDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountWoutPlan = listResult.Where(x => x.WOutPlanDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountTinPlan = listResult.Where(x => x.TInPlanDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountToutPlan = listResult.Where(x => x.TOutPlanDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountAinPlan = listResult.Where(x => x.AInPlanDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountAoutPlan = listResult.Where(x => x.AOutPlanDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountPdiDate = listResult.Where(x => x.PdiDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountLineoffdate = listResult.Where(x => x.LineoffDate >= input.DateFrom.Value.Date).Count();
            }
            else if (listResult.Count > 0 && input.DateFrom == null && input.DateTo != null)
            {
                listResult[0].CountWinPlan = listResult.Where(x =>  x.WInPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountWoutPlan = listResult.Where(x => x.WOutPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountTinPlan = listResult.Where(x =>  x.TInPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountToutPlan = listResult.Where(x =>  x.TOutPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountAinPlan = listResult.Where(x =>  x.AInPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountAoutPlan = listResult.Where(x =>  x.AOutPlanDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountPdiDate = listResult.Where(x =>  x.PdiDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountLineoffdate = listResult.Where(x =>  x.LineoffDate < input.DateTo.Value.Date.AddDays(1)).Count();
            }
            else if (listResult.Count > 0 && input.DateFrom == null && input.DateTo == null)
            {
                listResult[0].CountWinPlan = listResult.Count();
                listResult[0].CountWoutPlan = listResult.Count();
                listResult[0].CountTinPlan = listResult.Count();
                listResult[0].CountToutPlan = listResult.Count();
                listResult[0].CountAinPlan = listResult.Count();
                listResult[0].CountAoutPlan = listResult.Count();
                listResult[0].CountPdiDate = listResult.Count();
                listResult[0].CountLineoffdate = listResult.Count();
            }

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdProdPlanDailyDto>(
                totalCount,
                pagedAndFiltered
            );
        }


        public async Task<FileDto> GetProdPlanDailyToExcel(InvCkdProdPlanDailyExportInput input)
        {
            string _sql = "Exec INV_CKD_PROD_PLAN_DAILY_GETS @p_Vin, @p_LotNo, @p_NoInLot, @p_Cfc, " +
                "@p_BodyNo, @p_DateFrom, @p_DateTo, @p_IsPdiDate, @p_SelectDate";

            IEnumerable<InvCkdProdPlanDailyDto> result = await _dapperRepo.QueryAsync<InvCkdProdPlanDailyDto>(_sql, new
            {
                p_Vin = input.Vin,
                p_LotNo = input.LotNo,
                p_NoInLot = input.NoInLot,
                p_Cfc = input.Cfc,
                p_BodyNo = input.BodyNo,
                p_DateFrom = input.DateFrom,
                p_DateTo = input.DateTo,
                p_IsPdiDate = input.IsPdiDate,
                p_SelectDate = input.SelectDate
            });

            var listResult = result.ToList();

            var exportToExcel = result.ToList();
            return _prodPlanDailyExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetExportReportDaily(InvCkdProductionPlanDailyReportInput input)
        {

            var file = new FileDto("ReportProdPlanByDay.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ReportProdPlanByDay";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
            workSheet.Cells[0, 9].Value = "Report Prod Plan By Day " + input.FromDate.ToString("dd/MM/yyyy") + " - " + input.ToDate.ToString("dd/MM/yyyy");

            string _sql = "Exec INV_CKD_PROD_PLAN_DAILY_REPORT_BY_DAY @fromDate,@toDate, @isPdiDate";

            IEnumerable<InvCkdProductionPlanDailyReportDataDto> result = await _dapperRepo.QueryAsync<InvCkdProductionPlanDailyReportDataDto>(_sql, new
            {
                fromDate = input.FromDate,
                toDate = input.ToDate,
                isPdiDate = input.isPdiDate,
            });
            int dayColumnStart = 4;
            List<InvCkdProductionPlanDailyReportDataDto> listResult = result.ToList();
            int i = 0;
            for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddDays(1))
            {
                workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM-dd");

                i++;
            }

            List<InvCkdProductionPlanDailyReportDataDto> listDistinct = listResult.DistinctBy(e => new { e.CFC, e.Grade }).ToList();
            for (int j = 0; j < listDistinct.Count(); j++)
            {
                workSheet.Cells[3 + j, 0].Value = (j + 1);
                workSheet.Cells[3 + j, 1].Value = listDistinct[j].CFC;
                workSheet.Cells[3 + j, 2].Value = listDistinct[j].Grade;
                i = 0;
                for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddDays(1))
                {

                    workSheet.Cells[3 + j, dayColumnStart + i].Value = 0;

                    var valueCount = listResult.Where(e => e.CFC == listDistinct[j].CFC && e.Grade == listDistinct[j].Grade
                       && e.PdiDate.Date == day.Date).ToList();
                    if (valueCount.Count > 0)
                        workSheet.Cells[3 + j, dayColumnStart + i].Value =
                           valueCount[0].Count;

                    i++;
                }
            }

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }

        public async Task<FileDto> GetExportReportMonthly(InvCkdProductionPlanDailyReportInput input)
        {

            var file = new FileDto("ReportProdPlanByMonth.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ReportProdPlanByMonth";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
            workSheet.Cells[0, 9].Value = "Report Prod Plan By Month " + input.FromDate.ToString("MM/yyyy") + " - " + input.ToDate.ToString("MM/yyyy");

            string _sql = "Exec INV_CKD_PROD_PLAN_DAILY_REPORT_BY_MONTH @fromDate,@toDate,@isPdiDate";

            IEnumerable<InvCkdProductionPlanDailyReportDataDto> result = await _dapperRepo.QueryAsync<InvCkdProductionPlanDailyReportDataDto>(_sql, new
            {
                fromDate = input.FromDate,
                toDate = input.ToDate,
                isPdiDate = input.isPdiDate,
            });
            int dayColumnStart = 4;
            List<InvCkdProductionPlanDailyReportDataDto> listResult = result.ToList();
            int i = 0;
            for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddMonths(1))
            {
                workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM");

                i++;
            }

            List<InvCkdProductionPlanDailyReportDataDto> listDistinct = listResult.DistinctBy(e => new { e.CFC, e.Grade }).ToList();
            for (int j = 0; j < listDistinct.Count(); j++)
            {
                workSheet.Cells[3 + j, 0].Value = (j + 1);
                workSheet.Cells[3 + j, 1].Value = listDistinct[j].CFC;
                workSheet.Cells[3 + j, 2].Value = listDistinct[j].Grade;
                i = 0;
                for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddMonths(1))
                {

                    workSheet.Cells[3 + j, dayColumnStart + i].Value = 0;

                    var valueCount = listResult.Where(e => e.CFC == listDistinct[j].CFC && e.Grade == listDistinct[j].Grade
                       && e.PdiDateStr == day.ToString("yyyyMM")).ToList();
                    if (valueCount.Count > 0)
                        workSheet.Cells[3 + j, dayColumnStart + i].Value =
                           valueCount[0].Count;

                    i++;
                }
            }

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }


        public async Task<FileDto> GetExportReportColorDaily(InvCkdProductionPlanDailyReportInput input)
        {

            var file = new FileDto("ReportProdPlanColorByDay.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ReportProdPlanColorByDay";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
            workSheet.Cells[0, 9].Value = "Report Prod Plan Color By Day " + input.FromDate.ToString("dd/MM/yyyy") + " - " + input.ToDate.ToString("dd/MM/yyyy");

            string _sql = "Exec INV_CKD_PROD_PLAN_DAILY_REPORT_COLOR_BY_DAY @fromDate,@toDate,@isPdiDate";

            IEnumerable<InvCkdProductionPlanDailyReportDataDto> result = await _dapperRepo.QueryAsync<InvCkdProductionPlanDailyReportDataDto>(_sql, new
            {
                fromDate = input.FromDate,
                toDate = input.ToDate,
                isPdiDate = input.isPdiDate,
            });
            int dayColumnStart = 5;
            List<InvCkdProductionPlanDailyReportDataDto> listResult = result.ToList();
            int i = 0;
            for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddDays(1))
            {
                workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM-dd");

                i++;
            }

            List<InvCkdProductionPlanDailyReportDataDto> listDistinct = listResult.DistinctBy(e => new { e.CFC, e.Grade, e.Color }).ToList();
            for (int j = 0; j < listDistinct.Count(); j++)
            {
                workSheet.Cells[3 + j, 0].Value = (j + 1);
                workSheet.Cells[3 + j, 1].Value = listDistinct[j].CFC;
                workSheet.Cells[3 + j, 2].Value = listDistinct[j].Grade;
                workSheet.Cells[3 + j, 3].Value = listDistinct[j].Color;
                i = 0;
                for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddDays(1))
                {

                    workSheet.Cells[3 + j, dayColumnStart + i].Value = 0;

                    var valueCount = listResult.Where(e => e.CFC == listDistinct[j].CFC && e.Grade == listDistinct[j].Grade && e.Color == listDistinct[j].Color
                       && e.PdiDate.Date == day.Date).ToList();
                    if (valueCount.Count > 0)
                        workSheet.Cells[3 + j, dayColumnStart + i].Value =
                           valueCount[0].Count;

                    i++;
                }
            }

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }

        public async Task<FileDto> GetExportReportColorMonthly(InvCkdProductionPlanDailyReportInput input)
        {

            var file = new FileDto("ReportProdPlanColorByMonth.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ReportProdPlanColorByMonth";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
            workSheet.Cells[0, 9].Value = "Report Prod Plan Color By Month " + input.FromDate.ToString("MM/yyyy") + " - " + input.ToDate.ToString("MM/yyyy");

            string _sql = "Exec INV_CKD_PROD_PLAN_DAILY_REPORT_COLOR_BY_MONTH @fromDate,@toDate,@isPdiDate";

            IEnumerable<InvCkdProductionPlanDailyReportDataDto> result = await _dapperRepo.QueryAsync<InvCkdProductionPlanDailyReportDataDto>(_sql, new
            {
                fromDate = input.FromDate,
                toDate = input.ToDate,
                isPdiDate = input.isPdiDate,
            });
            int dayColumnStart = 5;
            List<InvCkdProductionPlanDailyReportDataDto> listResult = result.ToList();
            int i = 0;
            for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddMonths(1))
            {
                workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM");
                i++;
            }

            List<InvCkdProductionPlanDailyReportDataDto> listDistinct = listResult.DistinctBy(e => new { e.CFC, e.Grade, e.Color }).ToList();
            for (int j = 0; j < listDistinct.Count(); j++)
            {
                workSheet.Cells[3 + j, 0].Value = (j + 1);
                workSheet.Cells[3 + j, 1].Value = listDistinct[j].CFC;
                workSheet.Cells[3 + j, 2].Value = listDistinct[j].Grade;
                workSheet.Cells[3 + j, 3].Value = listDistinct[j].Color;
                i = 0;
                for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddMonths(1))
                {

                    workSheet.Cells[3 + j, dayColumnStart + i].Value = 0;

                    var valueCount = listResult.Where(e => e.CFC == listDistinct[j].CFC && e.Grade == listDistinct[j].Grade && e.Color == listDistinct[j].Color
                       && e.PdiDateStr == day.ToString("yyyyMM")).ToList();
                    if (valueCount.Count > 0)
                        workSheet.Cells[3 + j, dayColumnStart + i].Value =
                           valueCount[0].Count;

                    i++;
                }
            }

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }


    }



}
