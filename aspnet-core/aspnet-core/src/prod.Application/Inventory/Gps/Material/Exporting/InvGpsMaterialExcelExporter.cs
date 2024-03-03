using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Inventory.GPS.Dto;
using prod.Storage;
using prod.Inventory.GPS.Dto;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.Master.Pio.Exporting;
using System.IO;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using NPOI.Util.Collections;

namespace prod.Inventory.GPS.Exporting
{
    public class InvGpsMaterialExcelExporter : NpoiExcelExporterBase, IInvGpsMaterialExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;

        public InvGpsMaterialExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvGpsMaterialDto> Material)
        {
            return CreateExcelPackage(
                "InventoryGPSMaterial.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Material");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartNoNormalized"),
                                ("PartName"),
                                ("PartNameVn"),
                                ("ColorSfx"),
                                ("PartType"),
                                ("Category"),
                                ("Location"),
                                ("Purpose Of Use"),
                                ("Spec"),
                                ("HasExpiryDate"),
                                ("PartGroup"),
                                ("Price"),
                                ("Currency"),
                                ("PriceVND"),
                                ("Uom"),
                                ("SupplierName"),
                                ("Supplier"),
                                ("LocalImport"),
                                ("LeadTime"),
                                ("LeadTimeForecas"),
                                ("MinLot"),
                                ("BoxQty"),
                                ("Remark"),
                                ("PalletL"),
                                ("PalletR"),
                                ("PalletH"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, Material,
                                _ => _.PartNo,
                                _ => _.PartNoNormalized,
                                _ => _.PartName,
                                _ => _.PartNameVn,
                                _ => _.ColorSfx,
                                _ => _.PartType,
                                _ => _.Category,
                                _ => _.Location,
                                _ => _.PurposeOfUse,
                                _ => _.Spec,
                                _ => _.HasExpiryDate,
                                _ => _.PartGroup,
                                _ => _.Price,
                                _ => _.Currency,
                                _ => _.ConvertPrice,
                                _ => _.UOM,
                                _ => _.SupplierName,
                                _ => _.SupplierNo,
                                _ => _.LocalImport,
                                _ => _.LeadTime,
                                _ => _.LeadTimeForecast,
                                _ => _.MinLot,
                                _ => _.BoxQty,
                                _ => _.Remark,
                                _ => _.PalletL,
                                _ => _.PalletR,
                                _ => _.PalletH,
                                _ => _.IsActive

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
        public FileDto ExportToFileErr(List<ImportInvGpsMaterialDto> Material)
        {
            return CreateExcelPackage(
                "ListError_InventoryGPSMaterial.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Material");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("PartNo"),
                                ("PartNoNormalized"),
                                ("PartName"),
                                ("PartNameVn"),
                                ("ColorSfx"),
                                ("PartType"),
                                ("Category"),
                                ("Location"),
                                ("Purpose Of Use"),
                                ("Spec"),
                                ("HasExpiryDate"),
                                ("PartGroup"),
                                ("Price"),
                                ("Currency"),
                                ("PriceVND"),
                                ("Uom"),
                                ("SupplierName"),
                                ("Supplier"),
                                ("LocalImport"),
                                ("LeadTime"),
                                ("LeadTimeForecas"),
                                ("MinLot"),
                                ("BoxQty"),
                                ("Remark"),
                                ("PalletL"),
                                ("PalletR"),
                                ("PalletH"),
                                ("IsActive"),
                                ("Error Description")

                               );
                    AddObjects(
                         sheet, Material,
                                _ => _.ROW_NO,
                                _ => _.PartNo,
                                _ => _.PartNoNormalized,
                                _ => _.PartName,
                                _ => _.PartNameVn,
                                _ => _.ColorSfx,
                                _ => _.PartType,
                                _ => _.Category,
                                _ => _.Location,
                                _ => _.PurposeOfUse,
                                _ => _.Spec,
                                _ => _.HasExpiryDate,
                                _ => _.PartGroup,
                                _ => _.Price,
                                _ => _.Currency,
                                _ => _.ConvertPrice,
                                _ => _.UOM,
                                _ => _.SupplierName,
                                _ => _.SupplierNo,
                                _ => _.LocalImport,
                                _ => _.LeadTime,
                                _ => _.LeadTimeForecast,
                                _ => _.MinLot,
                                _ => _.BoxQty,
                                _ => _.Remark,
                                _ => _.PalletL,
                                _ => _.PalletR,
                                _ => _.PalletH,
                                _ => _.IsActive,
                                _ => _.ErrorDescription

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }


        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InventoryGpsMaterialHistorical.xlsx";
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(InvGpsMaterialExcelExporter)} with error: {ex}");
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
