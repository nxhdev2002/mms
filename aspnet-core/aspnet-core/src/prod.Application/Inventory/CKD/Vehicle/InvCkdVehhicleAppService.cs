using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using GemBox.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using prod.Assy.Andon;
using prod.Assy.Andon.Exporting;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Vehicle.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.Vehicle
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Area_Vehicle_View)]
    public class InvCkdVehhicleAppService : prodAppServiceBase, IInvCkdVehicleAppService
    {
        private readonly IDapperRepository<InvCkdStockPart, long> _dapperRepo;
        private readonly IRepository<AsyAdoVehicleDetails, long> _repo;
        private readonly IInvCkdVehicleExcelExporter _invCkdVehicle;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public InvCkdVehhicleAppService(IRepository<AsyAdoVehicleDetails, long> repo,
            IInvCkdVehicleExcelExporter invCkdVehicle,
             IDapperRepository<InvCkdStockPart, long> dapperRepo,
              ITempFileCacheManager tempFileCacheManager)
        {
            _repo = repo;
            _invCkdVehicle = invCkdVehicle;
            _dapperRepo = dapperRepo;
            _tempFileCacheManager = tempFileCacheManager;

        }


        // [AbpAuthorize(AppPermissions.Pages_Ckd_Area_Vehicle_Edit)]
        public async Task Update(InvCkdVehicleDto input)
        {
            string _sql = "Exec INV_CKD_VEHICLE_UPDATE @p_WInActualDateTime, @p_WOutActualDateTime, @p_TInActualDateTime, @p_TOutActualDateTime, @p_AInActualDateTime, @p_AOutActualDateTime," +
                "@p_LineoffDateTime, @p_PdiDateTime, @p_PIOActualDate, @p_SalesActualDate, @p_Id, @p_Grade, @p_LotNo, @p_UserId";

            IEnumerable<InvCkdVehicleDto> result = await _dapperRepo.QueryAsync<InvCkdVehicleDto>(_sql, new
            {
                p_WInActualDateTime =  input.WInActualDateTime,
                p_WOutActualDateTime = input.WOutActualDateTime,
                p_TInActualDateTime = input.TInActualDateTime,
                p_TOutActualDateTime = input.TOutActualDateTime,
                p_AInActualDateTime = input.AInActualDateTime,
                p_AOutActualDateTime = input.AOutActualDateTime,
                p_LineoffDateTime = input.LineoffDateTime,
                p_PdiDateTime = input.PdiDateTime,
                p_PIOActualDate = input.PIOActualDate,
                p_SalesActualDate = input.SalesActualDate,
                p_Id = input.Id,
                p_Grade = input.Grade,
                p_LotNo = input.LotNo,
                p_UserId = AbpSession.UserId
            });

          
        }



        public async Task<PagedResultDto<InvCkdVehicleDto>> GetAll(InvCkdVehicleInput input)
        {
            string _sql = "Exec INV_CKD_VEHICLE_GETS @p_Vin, @p_LotNo, @p_NoInLot, @p_Cfc, @p_BodyNo, " +
                "@p_Color, @p_SequenceNo, @p_DateFrom, @p_DateTo, @p_TypeDate, @p_SelectDate, @p_GoshiCar";

            IEnumerable<InvCkdVehicleDto> result = await _dapperRepo.QueryAsync<InvCkdVehicleDto>(_sql, new
            {
                p_Vin = input.Vin,
                p_LotNo = input.LotNo,
                p_NoInLot = input.NoInLot,
                p_Cfc = input.Cfc,
                p_BodyNo = input.BodyNo,
                p_Color = input.Color,
                p_SequenceNo = input.SequenceNo,
                p_DateFrom = input.DateFrom,
                p_DateTo = input.DateTo,
                p_TypeDate = input.TypeDate,
                p_SelectDate = input.SelectDate,
                p_GoshiCar = input.GoshiCar
            });

            var listResult = result.ToList();
            if (listResult.Count > 0 && input.DateFrom != null && input.DateTo != null)
            {
                    listResult[0].CountWin = listResult.Where(x => x.WInActualDate >= input.DateFrom.Value.Date && x.WInActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                    listResult[0].CountWout = listResult.Where(x => x.WOutActualDate >= input.DateFrom.Value.Date && x.WOutActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                    listResult[0].CountTin = listResult.Where(x => x.TInActualDate >= input.DateFrom.Value.Date && x.TInActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                    listResult[0].CountTout = listResult.Where(x => x.TOutActualDate >= input.DateFrom.Value.Date && x.TOutActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                    listResult[0].CountAin = listResult.Where(x => x.AInActualDate >= input.DateFrom.Value.Date && x.AInActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                    listResult[0].CountAout = listResult.Where(x => x.AOutActualDate >= input.DateFrom.Value.Date && x.AOutActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                    listResult[0].CountLineOut = listResult.Where(x => x.LineoffDate >= input.DateFrom.Value.Date && x.LineoffDate < input.DateTo.Value.Date.AddDays(1)).Count();
                    listResult[0].CountPdiDate = listResult.Where(x => x.PdiDate >= input.DateFrom.Value.Date && x.PdiDate < input.DateTo.Value.Date.AddDays(1)).Count();
                    listResult[0].CountPioDate = listResult.Where(x => x.PIOActualDate >= input.DateFrom.Value.Date && x.PIOActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                    listResult[0].CountSalesDate = listResult.Where(x => x.SalesActualDate >= input.DateFrom.Value.Date && x.SalesActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
            }
            if (listResult.Count > 0 && input.DateFrom != null && input.DateTo == null)
            {
                listResult[0].CountWin = listResult.Where(x => x.WInActualDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountWout = listResult.Where(x => x.WOutActualDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountTin = listResult.Where(x => x.TInActualDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountTout = listResult.Where(x => x.TOutActualDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountAin = listResult.Where(x => x.AInActualDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountAout = listResult.Where(x => x.AOutActualDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountLineOut = listResult.Where(x => x.LineoffDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountPdiDate = listResult.Where(x => x.PdiDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountPioDate = listResult.Where(x => x.PIOActualDate >= input.DateFrom.Value.Date).Count();
                listResult[0].CountSalesDate = listResult.Where(x => x.SalesActualDate >= input.DateFrom.Value.Date).Count();
            }
            if (listResult.Count > 0 && input.DateFrom == null && input.DateTo != null)
            {
                listResult[0].CountWin = listResult.Where(x =>  x.WInActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountWout = listResult.Where(x =>  x.WOutActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountTin = listResult.Where(x =>  x.TInActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountTout = listResult.Where(x =>  x.TOutActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountAin = listResult.Where(x =>  x.AInActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountAout = listResult.Where(x =>  x.AOutActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountLineOut = listResult.Where(x =>  x.LineoffDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountPdiDate = listResult.Where(x =>  x.PdiDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountPioDate = listResult.Where(x =>  x.PIOActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
                listResult[0].CountSalesDate = listResult.Where(x =>  x.SalesActualDate < input.DateTo.Value.Date.AddDays(1)).Count();
            }
            if (listResult.Count > 0 && input.DateFrom == null && input.DateTo == null)
            {
                listResult[0].CountWin = listResult.Count();
                listResult[0].CountWout = listResult.Count();
                listResult[0].CountTin = listResult.Count();
                listResult[0].CountTout = listResult.Count();
                listResult[0].CountAin = listResult.Count();
                listResult[0].CountAout = listResult.Count();
                listResult[0].CountLineOut = listResult.Count();
                listResult[0].CountPdiDate = listResult.Count();
                listResult[0].CountPioDate = listResult.Count();
                listResult[0].CountSalesDate = listResult.Count();
            }



            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdVehicleDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetInvVehicleToExcel(InvCkdVehicleInput input)
        {
            string _sql = "Exec INV_CKD_VEHICLE_GETS @p_Vin, @p_LotNo, @p_NoInLot, @p_Cfc, @p_BodyNo, " +
                "@p_Color, @p_SequenceNo, @p_DateFrom, @p_DateTo, @p_TypeDate, @p_SelectDate, @p_GoshiCar";

            IEnumerable<InvCkdVehicleDto> result = await _dapperRepo.QueryAsync<InvCkdVehicleDto>(_sql, new
            {
                p_Vin = input.Vin,
                p_LotNo = input.LotNo,
                p_NoInLot = input.NoInLot,
                p_Cfc = input.Cfc,
                p_BodyNo = input.BodyNo,
                p_Color = input.Color,
                p_SequenceNo = input.SequenceNo,
                p_DateFrom = input.DateFrom,
                p_DateTo = input.DateTo,
                p_TypeDate = input.TypeDate,
                p_SelectDate = input.SelectDate,
                p_GoshiCar = input.GoshiCar
            });

            var exportToExcel = result.ToList();

            return _invCkdVehicle.ExportToFile(exportToExcel);
        }


        public async Task<FileDto> GetCKDGIVehicleToExcel(InvCkdVehicleInput input)
        {
            string _sql = "Exec INV_CKD_ASY_ADO_VEHICLE_DETAILS_OUT_VEHICLE_BY_PDI @p_Vin, @p_LotNo, @p_NoInLot, " +
                "@p_Cfc, @p_BodyNo, @p_Color, @p_SequenceNo, @p_DateFrom, @p_DateTo, @p_TypeDate, @p_SelectDate, @p_GoshiCar";

            IEnumerable<InvCkdVehicleGIDto> result = await _dapperRepo.QueryAsync<InvCkdVehicleGIDto>(_sql, new
            {
                p_Vin = input.Vin,
                p_LotNo = input.LotNo,
                p_NoInLot = input.NoInLot,
                p_Cfc = input.Cfc,
                p_BodyNo = input.BodyNo,
                p_Color = input.Color,
                p_SequenceNo = input.SequenceNo,
                p_DateFrom = input.DateFrom,
                p_DateTo = input.DateTo,
                p_TypeDate = input.TypeDate,
                p_SelectDate = input.SelectDate,
                p_GoshiCar = input.GoshiCar
            });

            var exportToExcel = result.ToList();
            return _invCkdVehicle.ExportToFileGIVehicle(exportToExcel);
        }


        public async Task<FileDto> GetInvVehicleDetailOutPartExcel(InvCkdVehiclePDIInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_ASY_ADO_VEHICLE_DETAILS_OUT_PART_BY_PDI @p_Vin, @p_LotNo, @p_NoInLot, @p_Cfc, " +
                "@p_BodyNo, @p_Color, @p_SequenceNo, @p_DateFrom, @p_DateTo, @p_TypeDate, @p_SelectDate, @p_GoshiCar, @p_PartType";

            IEnumerable<InvCkdVehicleDetailOutPartDto> result = await _dapperRepo.QueryAsync<InvCkdVehicleDetailOutPartDto>(_sql, new
            {
                p_Vin = input.Vin,
                p_LotNo = input.LotNo,
                p_NoInLot = input.NoInLot,
                p_Cfc = input.Cfc,
                p_BodyNo = input.BodyNo,
                p_Color = input.Color,
                p_SequenceNo = input.SequenceNo,
                p_DateFrom = input.DateFrom,
                p_DateTo = input.DateTo,
                p_TypeDate = input.TypeDate,
                p_SelectDate = input.SelectDate,
                p_GoshiCar = input.GoshiCar,
                p_PartType = input.PartType
            });
            var exportToExcel = result.ToList();
            return _invCkdVehicle.DetailOutPartExportToFile(exportToExcel);
        }


        public async Task<FileDto> GetInvVehicleDetailOutPartWIPExcel(InvCkdVehiclePDIInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_ASY_ADO_VEHICLE_DETAILS_OUT_PART_BY_WIP @p_Vin, @p_LotNo, @p_NoInLot, @p_Cfc, " +
                "@p_BodyNo, @p_Color, @p_SequenceNo, @p_DateFrom, @p_DateTo, @p_TypeDate, @p_SelectDate, @p_GoshiCar, @p_PartType";

            IEnumerable<InvCkdVehicleDetailOutPartDto> result = await _dapperRepo.QueryAsync<InvCkdVehicleDetailOutPartDto>(_sql, new
            {
                p_Vin = input.Vin,
                p_LotNo = input.LotNo,
                p_NoInLot = input.NoInLot,
                p_Cfc = input.Cfc,
                p_BodyNo = input.BodyNo,
                p_Color = input.Color,
                p_SequenceNo = input.SequenceNo,
                p_DateFrom = input.DateFrom,
                p_DateTo = input.DateTo,
                p_TypeDate = input.TypeDate,
                p_SelectDate = input.SelectDate,
                p_GoshiCar = input.GoshiCar,
                p_PartType = input.PartType
            });
            var exportToExcel = result.ToList();
            return _invCkdVehicle.DetailOutPartExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetDetailsOutPartByVehicleToExcel(InvCkdOutPartByVehicleInput input)
        {
            string _sql = "Exec INV_CKD_ASY_ADO_VEHICLE_DETAILS_OUT_PART_BY_VEHICLE_V2 @p_lot_no, @p_no_in_lot, @p_part_type";

            IEnumerable<InvCkdOutPartByVehicleDto> result = await _dapperRepo.QueryAsync<InvCkdOutPartByVehicleDto>(_sql, new
            {
                p_lot_no = input.LotNo,
                p_no_in_lot = input.NoInLot,
                p_part_type = input.PartType
            });

            var exportToExcel = result.ToList();
            return _invCkdVehicle.OutPartByVehicleExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetExportReportDaily(InvCkdProductionActualReportInput input)
        {

            var file = new FileDto("ReportProdActualByDay.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ReportProdPlanByDay";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
            string p_title = "";

            if (input.p_mode == 0) p_title = "(Line Off Date) ";
            if (input.p_mode == 1) p_title = "(PDI Date) ";
            if (input.p_mode == 2) p_title = "(PIO Date) ";
            if (input.p_mode == 3) p_title = "(Sales Date) ";
            if (input.p_mode == 4) p_title = "(FG Stock Date) ";
            //if (input.p_mode == 1) p_title = "(Line Off Date) ";
            //else p_title = "(PDI Date) ";


            workSheet.Cells[0, 9].Value = "Report Prod Actual By Day " + p_title + input.FromDate.ToString("dd/MM/yyyy") + " - " + input.ToDate.ToString("dd/MM/yyyy");

            string _sql = "Exec INV_ASYADO_VEHICLE_DETAILS_REPORT_BY_DAY @p_mode,@fromDate,@toDate";

            IEnumerable<InvCkdProductionActualReportDataDto> result = await _dapperRepo.QueryAsync<InvCkdProductionActualReportDataDto>(_sql, new
            {
                p_mode = input.p_mode,
                fromDate = input.FromDate,
                toDate = input.ToDate,
            });
            int dayColumnStart = 4;
            List<InvCkdProductionActualReportDataDto> listResult = result.ToList();
            int i = 0;
            for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddDays(1))
            {
                workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM-dd");

                i++;
            }

            List<InvCkdProductionActualReportDataDto> listDistinct = listResult.DistinctBy(e => new { e.CFC, e.Grade }).ToList();
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
            obj_stream = new MemoryStream(System.IO.File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            System.IO.File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }

        public async Task<FileDto> GetExportReportMonthly(InvCkdProductionActualReportInput input)
        {

            var file = new FileDto("ReportProdActualByMonth.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ReportProdPlanByMonth";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
            string p_title = "";

            if (input.p_mode == 0) p_title = "(Line Off Date) ";
            if (input.p_mode == 1) p_title = "(PDI Date) ";
            if (input.p_mode == 2) p_title = "(PIO Date) ";
            if (input.p_mode == 3) p_title = "(Sales Date) ";
            if (input.p_mode == 4) p_title = "(FG Stock Date) ";
            //if (input.p_mode == 1) p_title = "(Line Off Date) ";
            //else p_title = "(PDI Date) ";


            workSheet.Cells[0, 9].Value = "Report Prod Actual By Month " + p_title + input.FromDate.ToString("MM/yyyy") + " - " + input.ToDate.ToString("MM/yyyy");

            string _sql = "Exec INV_ASYADO_VEHICLE_DETAILS_REPORT_BY_MONTH @p_mode,@fromDate,@toDate";

            IEnumerable<InvCkdProductionActualReportDataDto> result = await _dapperRepo.QueryAsync<InvCkdProductionActualReportDataDto>(_sql, new
            {
                p_mode = input.p_mode,
                fromDate = input.FromDate,
                toDate = input.ToDate,
            });
            int dayColumnStart = 4;
            List<InvCkdProductionActualReportDataDto> listResult = result.ToList();
            int i = 0;
            for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddMonths(1))
            {
                workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM");

                i++;
            }

            List<InvCkdProductionActualReportDataDto> listDistinct = listResult.DistinctBy(e => new { e.CFC, e.Grade }).ToList();
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
            obj_stream = new MemoryStream(System.IO.File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            System.IO.File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }


        public async Task<FileDto> GetExportReportColorDaily(InvCkdProductionActualReportInput input)
        {

            var file = new FileDto("ReportProdActualByColorDay.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ReportProdPlanColorByDay";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
            string p_title = "";
            if (input.p_mode == 0) p_title = "(Line Off Date) ";
            if (input.p_mode == 1) p_title = "(PDI Date) ";
            if (input.p_mode == 2) p_title = "(PIO Date) ";
            if (input.p_mode == 3) p_title = "(Sales Date) ";
            if (input.p_mode == 4) p_title = "(FG Stock Date) ";

            workSheet.Cells[0, 9].Value = "Report Prod Actual By Color Day " + p_title + input.FromDate.ToString("dd/MM/yyyy") + " - " + input.ToDate.ToString("dd/MM/yyyy");

            string _sql = "Exec INV_ASYADO_VEHICLE_DETAILS_REPORT_COLOR_BY_DAY @p_mode,@fromDate,@toDate";

            IEnumerable<InvCkdProductionActualReportDataDto> result = await _dapperRepo.QueryAsync<InvCkdProductionActualReportDataDto>(_sql, new
            {
                p_mode = input.p_mode,
                fromDate = input.FromDate,
                toDate = input.ToDate,
            });
            int dayColumnStart = 5;
            List<InvCkdProductionActualReportDataDto> listResult = result.ToList();
            int i = 0;
            for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddDays(1))
            {
                workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM-dd");

                i++;
            }

            List<InvCkdProductionActualReportDataDto> listDistinct = listResult.DistinctBy(e => new { e.CFC, e.Grade, e.Color }).ToList();
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
            obj_stream = new MemoryStream(System.IO.File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            System.IO.File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }

        public async Task<PagedResultDto<ViewIF>> GetViewIF(InvCkdVehicleViewIFInput input)
        {
            string _sql = "Exec ASY_ADO_VEHICLE_DETAILS_EXPORT_IF_LINE_OFF @p_fromdate,@p_todate";

            IEnumerable<ViewIF> result = await _dapperRepo.QueryAsync<ViewIF>(_sql, new
            {
                p_fromdate = input.DateFrom,
                p_todate = input.DateTo
            });
            var listResult = result.ToList();

            var totalCount = result.ToList().Count();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();


            return new PagedResultDto<ViewIF>(
                totalCount,
                pagedAndFiltered);

        }

        public async Task<FileDto> GetViewIFExcel(InvCkdVehicleViewIFInput input)
        {
            string _sql = "Exec ASY_ADO_VEHICLE_DETAILS_EXPORT_IF_LINE_OFF @p_fromdate,@p_todate";

            IEnumerable<ViewIF> result = await _dapperRepo.QueryAsync<ViewIF>(_sql, new
            {
                p_fromdate = input.DateFrom,
                p_todate = input.DateTo
            });
            var exportToExcel = result.ToList();
            return _invCkdVehicle.ListViewIFExportToFile(exportToExcel);

        }

        public async Task<FileDto> GetExportReportColorMonthly(InvCkdProductionActualReportInput input)
        {

            var file = new FileDto("ReportProdActualColorByMonth.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //if (input.FromDate == null || input.ToDate == null || input.ToDate.Date < input.FromDate.Date) return file;
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ReportProdPlanColorByMonth";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
            string p_title = "";

            if (input.p_mode == 0) p_title = "(Line Off Date) ";
            if (input.p_mode == 1) p_title = "(PDI Date) ";
            if (input.p_mode == 2) p_title = "(PIO Date) ";
            if (input.p_mode == 3) p_title = "(Sales Date) ";
            if (input.p_mode == 4) p_title = "(FG Stock Date) ";




            workSheet.Cells[0, 9].Value = "Report Prod Actual Color By Month " + p_title + input.FromDate.ToString("MM/yyyy") + " - " + input.ToDate.ToString("MM/yyyy");

            string _sql = "Exec INV_ASYADO_VEHICLE_DETAILS_REPORT_COLOR_BY_MONTH @p_mode,@fromDate,@toDate";

            IEnumerable<InvCkdProductionActualReportDataDto> result = await _dapperRepo.QueryAsync<InvCkdProductionActualReportDataDto>(_sql, new
            {
                p_mode = input.p_mode,
                fromDate = input.FromDate,
                toDate = input.ToDate,
                //isPdiDate = input.isPdiDate
            });
            int dayColumnStart = 5;
            List<InvCkdProductionActualReportDataDto> listResult = result.ToList();
            int i = 0;
            for (var day = input.FromDate; day.Date <= input.ToDate; day = day.AddMonths(1))
            {
                workSheet.Cells[2, dayColumnStart + i].Value = day.ToString("yyyy-MM");

                i++;
            }

            List<InvCkdProductionActualReportDataDto> listDistinct = listResult.DistinctBy(e => new { e.CFC, e.Grade, e.Color }).ToList();
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
            obj_stream = new MemoryStream(System.IO.File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            System.IO.File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }



        public async Task<FileDto> GetCkdVehicleByPeriodToExcel(int? periodId)
        {
            string _sql = "Exec INV_CKD_VEHICLE_DETAIL_BY_PERIOD @PeriodId";

            IEnumerable<InvCkdVehicleDto> result = await _dapperRepo.QueryAsync<InvCkdVehicleDto>(_sql, new
            {
                PeriodId = periodId
            });

            var exportToExcel = result.ToList();

            return _invCkdVehicle.ExportToFileByPeriod(exportToExcel);
        }
    }
}
