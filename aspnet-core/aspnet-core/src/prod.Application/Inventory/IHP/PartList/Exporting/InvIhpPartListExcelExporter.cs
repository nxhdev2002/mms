using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NPOI.HPSF;
using prod.Common;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.IHP.Dto;
using prod.Inventory.IHP.Exporting;
using prod.Inventory.IHP.PartGrade.Dto;
using prod.Master.Pio.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace prod.Inventory.IHP.PartList.Exporting
{
    public class InvIhpPartListExcelExporter : NpoiExcelExporterBase, IInvIhpPartListExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;

        public InvIhpPartListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvIhpPartListDto> ihppartlist)
        {
            return CreateExcelPackage(
                "InventoryIHPIhpPartList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("IhpPartList");
                    AddHeader(
                            sheet,
                            ("Supplier Type"),
                            ("Supplier Cd"),
                            ("Cfc"),
                            ("Part No"),
                            ("Part Name"),
                            ("Material Code"),
                            ("Material Spec"),
                            ("Sourcing"),
                            ("Cutting"),
                            ("Packing"),
                            ("Sheet Weight"),
                            ("Yiled Ration")
                        );
                    AddObjects(
                    sheet, ihppartlist,
                        _ => _.SupplierType,
                        _ => _.SupplierCd,
                        _ => _.Cfc,
                        _ => _.PartNo,
                        _ => _.PartName,
                        _ => _.MaterialCode,
                        _ => _.MaterialSpec,
                        _ => _.Sourcing,
                        _ => _.Cutting,
                        _ => _.Packing,
                        _ => _.SheetWeight,
                        _ => _.YiledRation
                        );
                });
        }

        public FileDto ExportPartGradeToFile(List<InvIhpPartGradeDto> invihppartgrade)
        {
            return CreateExcelPackage(
                "InventoryIHPInvIhpPartGrade.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvIhpPartGrade");
                    AddHeader(
                            sheet,
                            ("Grade"),
                            ("Usage Qty"),
                            ("First Day Product"),
                            ("Last Day Product")
                        );
                    AddObjects(
                    sheet, invihppartgrade,
                        _ => _.Grade,
                        _ => _.UsageQty,
                        _ => _.FirstDayProduct,
                        _ => _.LastDayProduct
                        );

                });

        }

        public FileDto ExportValidateToFile(List<ValidateIhpPartListDto> invIhppartlistvalidate)
        {
            return CreateExcelPackage(
                "InventoryIHPPartListValidate.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartList");
                    AddHeader(
                                sheet,
                                ("ErrorDescription"),
                                ("SupplierType"),
                                ("PartNo"),
                                ("PartName"),
                                ("Model"),
                                ("Cfc"),
                                ("MaterialId"),
                                ("MaterialCode"),
                                ("Packing"),
                                ("SheetWeight"),
                                ("YiledRation"),
                                ("Grade"),
                                ("UsageQty"),
                                ("FirstDayProduct"),
                                ("LastDayProduct"),
                                ("PartListId"),
                                ("PartGradeId"),
                                ("DrmPartListId")
                               );
                    AddObjects(
                         sheet, invIhppartlistvalidate,
                             _ => _.ErrorDescription,
                             _ => _.SupplierType,
                             _ => _.PartNo,
                             _ => _.PartName,
                             _ => _.Model,
                             _ => _.Cfc,
                             _ => _.MaterialId,
                             _ => _.MaterialCode,
                             _ => _.Packing,
                             _ => _.SheetWeight,
                             _ => _.YiledRation,
                             _ => _.Grade,
                             _ => _.UsageQty,
                             _ => _.FirstDayProduct,
                             _ => _.LastDayProduct,
                             _ => _.PartListId,
                             _ => _.PartGradeId,
                             _ => _.DrmPartListId
                                );


                });

        }

        public FileDto ExportToHistoricalFile(List<string> data, string tableName)
        {
            string fileName = "";
            if (tableName == "InvIhpPartList")
            {
                fileName = "InventoryIhpPartListHistorical.xlsx";
            }
            else if (tableName == "InvIhpPartGrade")
            {
                fileName = "InventoryIhpPartGradeHistorical.xlsx";
            }
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            var allHeaders = new List<string>();
            var rowDatas = new List<JObject>();
            var exceptCols = new List<string>()
            {
                "UpdateMask",
                "LastModificationTime",
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
                        if (!allHeaders.Contains(prop.Name) && !exceptCols.Contains(prop.Name))
                        {
                            allHeaders.Add(prop.Name);
                        }
                    }
                }

                var properties = allHeaders.Where(x => !exceptCols.Contains(x)).ToArray();


                /// Mapping Data
                MappingData(ref rowDatas, ref properties);

                Commons.FillHistoriesExcel(rowDatas, workSheet, 1, 0, properties, properties);
                xlWorkBook.Save(tempFile);
                using (var obj_stream = new MemoryStream(File.ReadAllBytes(tempFile)))
                {
                    _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[EXCEPTION] While exporting {nameof(MstLspSupplierInforExporter)} with error: {ex}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            return file;
        }

        private void MappingData(ref List<JObject> rowDatas, ref string[] properties)
        {
            /// Mapping row data
            /// add new here
            var dataMapping = new Dictionary<string, Dictionary<string, string>>()
            {
                {
                    "Action", new Dictionary<string, string>() {
                        {"1", "Xoá"},
                        {"2", "Tạo mới"},
                        {"3", "Trước update"},
                        {"4", "Sau update"},
                    }
                    //"Header1", new Dictionary<string, string>()
                    //{
                    //    {"1", "Xoá"},
                    //    {"2", "Tạo mới"},
                    //    {"3", "Trước update"},
                    //    {"4", "Sau update"},
                    //},
                },
            };

            foreach (var header in dataMapping)
            {
                rowDatas.ConvertAll(x =>
                    x[header.Key] = dataMapping[header.Key][x[header.Key].ToString()]
                );
            }
        }
    }
}
