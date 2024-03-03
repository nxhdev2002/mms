
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Master.Pio.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdPartListExcelExporter : NpoiExcelExporterBase, IInvCkdPartListExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public InvCkdPartListExcelExporter(ITempFileCacheManager tempFileCacheManager,
            ILogger<InvCkdPartListExcelExporter> logger

            ) : base(tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _logger = logger;

        }
        public FileDto ExportToFile(List<InvCkdPartListDto> partlist)
        {
            return CreateExcelPackage(
                "InventoryCKDPartList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartList");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartNoNormalized"),
                                ("PartName"),
                                ("SupplierNo"),
                                ("SupplierCd"),
                                ("ColorSfx"),
                                ("SupplierId"),
                                ("MaterialId"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet, partlist,
                                _ => _.PartNo,
                                _ => _.PartNoNormalized,
                                _ => _.PartName,
                                _ => _.SupplierNo,
                                _ => _.SupplierCd,
                                _ => _.ColorSfx,
                                _ => _.SupplierId,
                                _ => _.MaterialId,
                                _ => _.IsActive
                                );


                });

        }
        public FileDto ExportValidateToFile(List<ValidatePartListDto> invckdpartlistvalidate)
        {
            return CreateExcelPackage(
                "InventoryCKDPartListValidate.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartList");
                    AddHeader(
                                sheet,
                                ("PartListId"),
                                 ("PartGradeId"),
                                ("ErrorDescription"),
                                ("PartNo"),
                                ("PartNoNormalizedS4"),
                                ("PartName"),
                                ("SupplierNo"),
                                ("Model"),
                                ("Cfc"),
                                ("MaterialId"),
                                ("OrderPattern"),
                                ("Grade"),
                                ("Shop"),
                                ("BodyColor"),
                                ("UsageQty"),
                                ("StartLot"),
                                ("EndLot"),
                                ("StartRun"),
                                ("EndRun"),
                                ("CreationTime"),
                                ("CreatorUserId")
                               );
                    AddObjects(
                         sheet, invckdpartlistvalidate,
                             _ => _.PartListId,
                             _ => _.PartGradeId,
                             _ => _.ErrorDescription,
                             _ => _.PartNo,
                             _ => _.PartNoNormalizedS4,
                             _ => _.PartName,
                             _ => _.SupplierNo,
                             _ => _.Model,
                             _ => _.Cfc,
                             _ => _.MaterialId,
                             _ => _.OrderPattern,
                             _ => _.Grade,
                             _ => _.Shop,
                             _ => _.BodyColor,
                             _ => _.UsageQty,
                             _ => _.StartLot,
                             _ => _.EndLot,
                             _ => _.StartRun,
                             _ => _.EndRun,
                             _ => _.CreationTime,
                             _ => _.CreatorUserId
                                );


                });

        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InventoryCKDPartListHistorical.xlsx";
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            var allHeaders = new List<string>();
            var rowDatas = new List<JObject>();
            var exceptCols = new List<string>()
            {
                "UpdateMask",
         
            };

            try
            {
                SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                var xlWorkBook = new ExcelFile();
                var workSheet = xlWorkBook.Worksheets.Add("Sheet1");

                foreach (var item in data)
                {
                    var json = JObject.Parse(item);
                    rowDatas.Add(json);
                    foreach (var prop in json.Properties())
                    {
                        if (prop.Name == "Action")
                        {

                            switch (prop.Value.ToString())
                            {
                                case "1":
                                    prop.Value = "Xóa";
                                    break;
                                case "2":
                                    prop.Value = "Tạo mới";
                                    break;
                                case "3":
                                    prop.Value = "Trước Update";
                                    break;
                                case "4":
                                    prop.Value = "Sau Update";
                                    break;
                            }
                        }
                        if (!allHeaders.Contains(prop.Name) && !exceptCols.Contains(prop.Name))
                        {
                            allHeaders.Add(prop.Name);
                        }
                    }
                }

                var ins = new object();
                var properties = allHeaders.Where(x => !exceptCols.Contains(x))
                                            .ToArray();

                Commons.FillHistoriesExcel(rowDatas, workSheet, 1, 0, properties, properties);
                xlWorkBook.Save(tempFile);
                using (var obj_stream = new MemoryStream(File.ReadAllBytes(tempFile)))
                {
                    _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[EXCEPTION] While exporting {nameof(InvCkdPartListExcelExporter)} with error: {ex}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            return file;
        }



        public FileDto ExportToFileErr(List<ImportCkdPartListDto> errpartlist)
        {
            return CreateExcelPackage(
                "ListErrorImportCKdPartList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartListError");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("OrderPattern"),
                                ("Common"),
                                ("Type"),
                                ("Cfc"),
                                ("Shop"),
                                ("IdLine"),
                                ("PartNo"),
                                ("PartName"),
                                ("SupplierNo"),
                                ("SupplierCd"),
                                ("BodyColor"),
                                ("BackNo"),
                                ("ModuleNo"),
                                ("Renban"),
                                ("ReExportCd"),
                                ("IcoFlag"),
                                ("StartPackingMonth"),
                                ("EndPackingMonth"),
                                ("StartProductionMonth"),
                                ("EndProductionMonth"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, errpartlist,
                                _ => _.ROW_NO,
                                _ => _.OrderPattern,
                                _ => _.Common,
                                _ => _.Type,
                                _ => _.Cfc,
                                _ => _.Shop,
                                _ => _.IdLine,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.SupplierNo,
                                _ => _.SupplierCd,
                                _ => _.BodyColor,
                                _ => _.BackNo,
                                _ => _.ModuleNo,
                                _ => _.Renban,
                                _ => _.ReExportCd,
                                _ => _.IcoFlag,
                                _ => _.StartPackingMonth,
                                _ => _.EndPackingMonth,
                                _ => _.StartProductionMonth,
                                _ => _.EndProductionMonth,
                                _ => _.ErrorDescription
                                );


                });

        }
        public FileDto ExportToFileLotErr(List<ImportCkdPartListDto> errpartlist)
        {
            return CreateExcelPackage(
                "ListErrorImportCKdPartLot.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartLotError");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Model"),
                                ("Shop"),
                                ("PartNo"),
                                ("PartName"),
                                ("Source"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, errpartlist,
                                _ => _.ROW_NO,
                                _ => _.Cfc,
                                _ => _.Shop,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.SupplierNo,
                                _ => _.ErrorDescription
                                );


                });

        }

        public FileDto ExportToFileErrGrade(List<ImportInvCkdPartGradeDto> listerror)
        {
            return CreateExcelPackage(
                "ListErrorImportCKdPartGrade.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ListErrorImportPartGrade");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("MaintenanceCode"),
                                ("PartStatus"),
                                ("Model"),
                                ("Shop"),
                                ("IdLine"),
                                ("PartNo"),
                                ("PartName"),
                                ("Exporter"),
                                ("ExporterCode"),
                                ("BodyColor"),
                                ("Grade"),
                                ("UsageQty"),
                                ("StartLot"),
                                ("StartNoInLot"),
                                ("EndLot"),
                                ("EndNoInLot"),
                                ("ECIFromPart"),
                                ("ECIToPart"),
                                ("OrderPattern"),
                                ("Remark"),
                                ("UpdateDate"),
                                ("UpdateUser"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, listerror,
                                _ => _.ROW_NO,
                                _ => _.MaintenanceCode,
                                _ => _.PartStatus,
                                _ => _.Cfc,
                                _ => _.Shop,
                                _ => _.IdLine,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Exporter,
                                _ => _.ExporterCode,
                                _ => _.BodyColor,
                                _ => _.Grade,
                                _ => _.UsageQty,
                                _ => _.StartLot,
                                _ => _.StartNoInLot,
                                _ => _.EndLot,
                                _ => _.EndNoInLot,
                                _ => _.ECIFromPart,
                                _ => _.ECIToPart,
                                _ => _.OrderPattern,
                                _ => _.Remark,
                                _ => _.UpdateDate,
                                _ => _.UpdateUser,
                                _ => _.ErrorDescription
                                );
                });
        }
    }
}
