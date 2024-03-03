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
using prod.Inventory.Tmss.Dto;
using prod.Inventory.Tmss.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.Tmss
{
	[AbpAuthorize(AppPermissions.Pages_PIO_Master_Tmss_TmssDispatchPlan_View)]
	public class InvTmssDispatchPlanAppService : prodAppServiceBase, IInvTmssDispatchPlanAppService
	{
		private readonly IDapperRepository<InvTmssDispatchPlan, long> _dapperRepo;
		private readonly IRepository<InvTmssDispatchPlan, long> _repo;
		private readonly IInvTmssDispatchPlanExcelExporter _calendarListExcelExporter;
		private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IHistoricalDataAppService _historicalDataAppService;


        public InvTmssDispatchPlanAppService(IRepository<InvTmssDispatchPlan, long> repo,
										 IDapperRepository<InvTmssDispatchPlan, long> dapperRepo,
										IInvTmssDispatchPlanExcelExporter calendarListExcelExporter,
										ITempFileCacheManager tempFileCacheManager,
                                        IHistoricalDataAppService historicalDataAppService
			)
		{
			_repo = repo;
			_dapperRepo = dapperRepo;
			_calendarListExcelExporter = calendarListExcelExporter;
			_tempFileCacheManager = tempFileCacheManager;
			_historicalDataAppService = historicalDataAppService;
		}
        public async Task<List<string>> GetInvTmssDispatchPlanHistory(GetInvTmssDispatchPlanHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvTmssDispatchPlanHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("InvTmssDispatchPlan");
        }

        public async Task<PagedResultDto<InvTmssDispatchPlanDto>> GetAll(GetInvTmssDispatchPlanInput input)
		{
			string _sql = "Exec INV_TMSS_DISPATCH_PLAN_SEARCH @p_vehicle_type,@p_model,@p_marketing_code,@p_dispatch_plan_date_from,@p_dispatch_plan_date_to,@p_dispatch_date_from,@p_dispatch_date_to,@p_vin";

			IEnumerable<InvTmssDispatchPlanDto> result = await _dapperRepo.QueryAsync<InvTmssDispatchPlanDto>(_sql, new
			{
				p_vehicle_type = input.VehicleType,
				p_model = input.Model,
				p_marketing_code = input.MarketingCode,
				p_dispatch_plan_date_from = input.DlrDispatchPlanDateFrom,
				p_dispatch_plan_date_to = input.DlrDispatchPlanDateTo,
				p_dispatch_date_from = input.DlrDispatchDateFrom,
				p_dispatch_date_to = input.DlrDispatchDateTo,
				p_vin = input.Vin
			});

			var listResult = result.ToList();

			var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

			var totalCount = result.ToList().Count();

			return new PagedResultDto<InvTmssDispatchPlanDto>(
				totalCount,
				pagedAndFiltered);
		}


		public async Task<FileDto> GetTmssDispatchPlanToExcel(InvTmssDispatchPlanExportInput input)
		{
			string _sql = "Exec INV_TMSS_DISPATCH_PLAN_SEARCH @p_vehicle_type,@p_model,@p_marketing_code,@p_dispatch_plan_date_from,@p_dispatch_plan_date_to,@p_dispatch_date_from,@p_dispatch_date_to,@p_vin";

			IEnumerable<InvTmssDispatchPlanDto> result = await _dapperRepo.QueryAsync<InvTmssDispatchPlanDto>(_sql, new
			{
				p_vehicle_type = input.VehicleType,
				p_model = input.Model,
				p_marketing_code = input.MarketingCode,
				p_dispatch_plan_date_from = input.DlrDispatchPlanDateFrom,
				p_dispatch_plan_date_to = input.DlrDispatchPlanDateTo,
				p_dispatch_date_from = input.DlrDispatchDateFrom,
				p_dispatch_date_to = input.DlrDispatchDateTo,
				p_vin = input.Vin
			});
			var exportToExcel = result.ToList();
			return _calendarListExcelExporter.ExportToFile(exportToExcel);
		}

		public async Task<FileDto> GetExportReportDaily(GetInvPIOTmssDispatchPlanInput input)
		{

			var file = new FileDto("ReportDispatPlanByDay.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
			//if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
			SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
			string fileName = "temp_ReportDispatchPlanByDay";
			string template = "wwwroot/Template";
			string path = "";
			path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
			var xlWorkBook = ExcelFile.Load(path + ".xlsx");
			var workSheet = xlWorkBook.Worksheets[0];
			workSheet.Cells[0, 9].Value = "Report Dispat Plan By Day " + input.FromDate.ToString("dd/MM/yyyy") + " - " + input.ToDate.ToString("dd/MM/yyyy");

			string _sql = "Exec INV_PIO_DISPATCH_PLAN_DAILY_REPORT_BY_DAY @fromDate,@toDate";

			IEnumerable<InvPIOTmssDispatchPlanDailyReportDataDto> result = await _dapperRepo.QueryAsync<InvPIOTmssDispatchPlanDailyReportDataDto>(_sql, new
			{
				fromDate = input.FromDate,
				toDate = input.ToDate,
			});

			int dayColumnStart = 4;


			//Writing Header
			List<InvPIOTmssDispatchPlanDailyReportDataDto> listResult = result.ToList();
			int i = 0;
			for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddDays(1))
			{
				workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM-dd");

				i++;
			}


			var listDistinctMarketingCode = listResult.DistinctBy(e => new { e.MarketingCode, e.DlrDispatchPlan })
											.Select(x => new { x.MarketingCode, x.DlrDispatchPlan }).ToList();

			for (int j = 0; j < listDistinctMarketingCode.Count(); j++)
			{
				workSheet.Cells[3 + j, 0].Value = (j + 1); // writing sequence

				workSheet.Cells[3 + j, 1].Value = listDistinctMarketingCode[j].MarketingCode;
				i = 0;
				for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddDays(1))
				{

					workSheet.Cells[3 + j, dayColumnStart + i].Value = 0;

					var valueCount = listResult.Where(e => e.MarketingCode == listDistinctMarketingCode[j].MarketingCode && e.DlrDispatchPlan.Date == day.Date
					  ).ToList();
					if (valueCount.Count > 0)
						workSheet.Cells[3 + j, dayColumnStart + i].Value =
						   valueCount.First().Count;

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
		public async Task<FileDto> GetExportReportMarketingCodeMonthly(GetInvPIOTmssDispatchPlanInput input)
		{

			var file = new FileDto("ReportMarketingCodePlanByMonth.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
			//if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
			SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
			string fileName = "temp_ReportMarketingCodePlanByMonth";
			string template = "wwwroot/Template";
			string path = "";
			path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
			var xlWorkBook = ExcelFile.Load(path + ".xlsx");
			var workSheet = xlWorkBook.Worksheets[0];
			workSheet.Cells[0, 9].Value = "Report Marketing Code Plan By Month " + input.FromDate.ToString("MM/yyyy") + " - " + input.ToDate.ToString("MM/yyyy");

			string _sql = "Exec INV_PIO_DISPATCH_PLAN_DAILY_REPORT_BY_MONTH @fromDate,@toDate";

			IEnumerable<InvPIOTmssDispatchPlanDailyReportDataDto> result = await _dapperRepo.QueryAsync<InvPIOTmssDispatchPlanDailyReportDataDto>(_sql, new
			{
				fromDate = input.FromDate.Date,
				toDate = input.ToDate.Date,
			});
			int dayColumnStart = 4;

			// Writing Header
			List<InvPIOTmssDispatchPlanDailyReportDataDto> listResult = result.ToList();
			  int i = 0;
			for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddMonths(1))
			{
				workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM");

				i++;
			}

			//List<InvPIOTmssDispatchPlanDailyReportDataDto> listDistinct = listResult.DistinctBy(e => new { e.CFC, e.Grade }).ToList();
			var listDistinctMarketingCode = listResult.DistinctBy(e => new { e.MarketingCode, e.DlrDispatchPlan })
											.Select(x => new { x.MarketingCode, x.DlrDispatchPlan }).ToList();
			for (int j = 0; j < listDistinctMarketingCode.Count(); j++)
			{
				workSheet.Cells[3 + j, 0].Value = (j + 1);
				workSheet.Cells[3 + j, 1].Value = listDistinctMarketingCode[j].MarketingCode;
				i = 0;
				for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddMonths(1))
				{

					workSheet.Cells[3 + j, dayColumnStart + i].Value = 0;

					var valueCount = listResult.Where(e => e.MarketingCode == listDistinctMarketingCode[j].MarketingCode 
					   && e.DlrMonth == day.ToString("yyyyMM")).ToList();
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

		public async Task<FileDto> GetExportReportColorDaily(GetInvPIOTmssDispatchPlanInput input)
		{

			var file = new FileDto("ReportDispatPlanByColorByDay.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
			//if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
			SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
			string fileName = "temp_ReportDispatchPlanByColorByDay";
			string template = "wwwroot/Template";
			string path = "";
			path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
			var xlWorkBook = ExcelFile.Load(path + ".xlsx");
			var workSheet = xlWorkBook.Worksheets[0];
			workSheet.Cells[0, 9].Value = "Report ExtColor By Day " + input.FromDate.ToString("dd/MM/yyyy") + " - " + input.ToDate.ToString("dd/MM/yyyy");

			string _sql = "Exec INV_PIO_DISPATCH_PLAN_DAILY_REPORT_Color_BY_DAY @fromDate,@toDate";

			IEnumerable<InvPIOTmssDispatchPlanDailyReportDataDto> result = await _dapperRepo.QueryAsync<InvPIOTmssDispatchPlanDailyReportDataDto>(_sql, new
			{
				fromDate = input.FromDate,
				toDate = input.ToDate,
			});

			int dayColumnStart = 4;


			//Writing Header
			List<InvPIOTmssDispatchPlanDailyReportDataDto> listResult = result.ToList();
			int i = 0;
			for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddDays(1))
			{
				workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM-dd");

				i++;
			}


			var listDistinctMarketingCode = listResult.DistinctBy(e => new { e.MarketingCode,e.IntColor, e.DlrDispatchPlan })
											.Select(x => new {x.MarketingCode, x.IntColor, x.DlrDispatchPlan }).ToList();

			for (int j = 0; j < listDistinctMarketingCode.Count(); j++)
			{
				workSheet.Cells[3 + j, 0].Value = (j + 1); // writing sequence
				workSheet.Cells[3 + j, 1].Value = listDistinctMarketingCode[j].MarketingCode;

				workSheet.Cells[3 + j, 2].Value = listDistinctMarketingCode[j].IntColor;
				i = 0;
				for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddDays(1))
				{

					workSheet.Cells[3 + j, dayColumnStart + i].Value = 0;

					var valueCount = listResult.Where(e => e.IntColor == listDistinctMarketingCode[j].IntColor && e.DlrDispatchPlan.Date == day.Date
					  ).ToList();
					if (valueCount.Count > 0)
						workSheet.Cells[3 + j, dayColumnStart + i].Value =
						   valueCount.First().Count;

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

		public async Task<FileDto> GetExportReportColorMonthly(GetInvPIOTmssDispatchPlanInput input)
		{

			var file = new FileDto("ReportDispatchPlanByColorByMonth.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
			//if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
			SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
			string fileName = "temp_ReportDispatchPlanByColorByMonth";
			string template = "wwwroot/Template";
			string path = "";
			path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
			var xlWorkBook = ExcelFile.Load(path + ".xlsx");
			var workSheet = xlWorkBook.Worksheets[0];
			workSheet.Cells[0, 9].Value = "Report Dispatch Plan By Color By Month " + input.FromDate.ToString("MM/yyyy") + " - " + input.ToDate.ToString("MM/yyyy");

			string _sql = "Exec INV_PIO_DISPATCH_PLAN_DAILY_REPORT_COLOR_BY_MONTH @fromDate,@toDate";

			IEnumerable<InvPIOTmssDispatchPlanDailyReportDataDto> result = await _dapperRepo.QueryAsync<InvPIOTmssDispatchPlanDailyReportDataDto>(_sql, new
			{
				fromDate = input.FromDate.Date,
				toDate = input.ToDate.Date,
			});
			int dayColumnStart = 4;

			// Writing Header
			List<InvPIOTmssDispatchPlanDailyReportDataDto> listResult = result.ToList();
			int i = 0;
			for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddMonths(1))
			{
				workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM");

				i++;
			}

			//List<InvPIOTmssDispatchPlanDailyReportDataDto> listDistinct = listResult.DistinctBy(e => new { e.CFC, e.Grade }).ToList();
			var listDistinctMarketingCode = listResult.DistinctBy(e => new { e.MarketingCode,e.IntColor, e.DlrDispatchPlan })
											.Select(x => new { x.MarketingCode,x.IntColor, x.DlrDispatchPlan }).ToList();
			for (int j = 0; j < listDistinctMarketingCode.Count(); j++)
			{
				workSheet.Cells[3 + j, 0].Value = (j + 1);
				workSheet.Cells[3 + j, 1].Value = listDistinctMarketingCode[j].MarketingCode;
				workSheet.Cells[3 + j, 2].Value = listDistinctMarketingCode[j].IntColor;
				i = 0;
				for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddMonths(1))
				{

					workSheet.Cells[3 + j, dayColumnStart + i].Value = 0;

					var valueCount = listResult.Where(e => e.MarketingCode == listDistinctMarketingCode[j].MarketingCode
					   && e.DlrMonth == day.ToString("yyyyMM")).ToList();
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
