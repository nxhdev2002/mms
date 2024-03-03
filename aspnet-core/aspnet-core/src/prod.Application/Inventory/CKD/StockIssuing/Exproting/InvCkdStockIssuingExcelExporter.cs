using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
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
using Microsoft.Extensions.Logging;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdStockIssuingExcelExporter : NpoiExcelExporterBase, IInvCkdStockIssuingExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public InvCkdStockIssuingExcelExporter(ITempFileCacheManager tempFileCacheManager, ILogger<InvCkdPartListExcelExporter> logger) : base(tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _logger = logger;
        }
        public FileDto ExportToFile(List<InvCkdStockIssuingDto> stockissuing)
        {

            var file = new FileDto("InventoryCKDStockIssuing.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InventoryCKDStockIssuing";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("PartNo");
            listHeader.Add("ColorSfx");
            listHeader.Add("Cfc");
            listHeader.Add("SupplierNo");
            listHeader.Add("LotNo");
            listHeader.Add("NoInLot");
            listHeader.Add("Qty");
            listHeader.Add("VinNo");
            listHeader.Add("BodyNo");
            listHeader.Add("Color");
            listHeader.Add("PartName");
            listHeader.Add("TransactionDatetime");
            listHeader.Add("WorkingDate");
            listHeader.Add("MaterialId");


            List<string> listExport = new List<string>();
            listExport.Add("PartNo");
            listExport.Add("ColorSfx");
            listExport.Add("Cfc");
            listExport.Add("SupplierNo");
            listExport.Add("LotNo");
            listExport.Add("NoInLot");
            listExport.Add("Qty");
            listExport.Add("VinNo");
            listExport.Add("BodyNo");
            listExport.Add("Color");
            listExport.Add("PartName");
            listExport.Add("TransactionDatetime");
            listExport.Add("WorkingDate");
            listExport.Add("MaterialId");



            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(stockissuing, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;


        }

        public FileDto ExportByMaterialToFile(List<InvCkdStockPartByMaterialDto> invckdstockissuingbymaterial)
        {
            return CreateExcelPackage(
                "InvCkdStockIssuingByMaterial.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StockIssuingByMasterial");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("Description"),
                                ("ValuationType"),
                                ("Qty"),
                                ("WorkingDate"),
                                ("LastCalDatetime"),
                                ("MaterialId")
                               );
                    AddObjects(
                         sheet, invckdstockissuingbymaterial,
                                _ => _.PartNo,
                                _ => _.Description,
                                _ => _.ValuationType,
                                _ => _.Qty,
                                _ => _.WorkingDate,
                                _ => _.LastCalDatetime,
                                _ => _.MaterialId
                                );


                });

        }
        public FileDto ExportStockIssuingViewToFile(List<InvCkdStockIssuingTranslocDto> stockissuing)
        {
            return CreateExcelPackage(
                "InventoryCKDStockIssuingTransloc.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StockIssuing");
                    AddHeader(
                                sheet,
                                ("RunningNo"),
                                ("VinNo"),
                                ("DocumentDate"),
                                ("PostingDate"),
                                ("DocumentHeaderText"),
                                ("MovementType"),
                                ("MaterialCodeFrom"),
                                ("PlantFrom"),
                                ("ValuationTypeFrom"),
                                ("StorageLocationFrom"),
                                ("ProductionVersion"),
                                ("Quantity"),
                                ("UnitOfEntry"),
                                ("ItemText"),
                                ("GlAccount"),
                                ("CostCenter"),
                                ("Wbs"),
                                ("MaterialCodeTo"),
                                ("PlantTo"),
                                ("ValuationTypeTo"),
                                ("StorageLocationTo"),
                                ("BfPc"),
                                ("CancelFlag"),
                                ("ReffMatDocNo"),
                                ("VendorNo"),
                                ("ProfitCenter"),
                                ("ShipemntCat"),
                                ("Reference"),
                                ("AssetNo"),
                                ("SubAssetNo"),
                                ("EndOfRecord")

                               );
                    AddObjects(
                         sheet, stockissuing,
                                _ => _.RunningNo,
                                _ => _.VinNo,
                                _ => _.DocumentDate,
                                _ => _.PostingDate,
                                _ => _.DocumentHeaderText,
                                _ => _.MovementType,
                                _ => _.MaterialCodeFrom,
                                _ => _.PlantFrom,
                                _ => _.ValuationTypeFrom,
                                _ => _.StorageLocationFrom,
                                _ => _.ProductionVersion,
                                _ => _.Quantity,
                                _ => _.UnitOfEntry,
                                _ => _.ItemText,
                                _ => _.GlAccount,
                                _ => _.CostCenter,
                                _ => _.Wbs,
                                _ => _.MaterialCodeTo,
                                _ => _.PlantTo,
                                _ => _.ValuationTypeTo,
                                _ => _.StorageLocationTo,
                                _ => _.BfPc,
                                _ => _.CancelFlag,
                                _ => _.ReffMatDocNo,
                                _ => _.VendorNo,
                                _ => _.ProfitCenter,
                                _ => _.ShipemntCat,
                                _ => _.Reference,
                                _ => _.AssetNo,
                                _ => _.SubAssetNo,
                                _ => _.EndOfRecord

                                );


                });

        }

        public FileDto ExportStockIssuingValidate(List<InvCkdStockIssuingValidateDto> checkstockissuing)
        {
            return CreateExcelPackage(
                "InventoryCKDStockIssuingValidate.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StockIssuing");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("SupplierNo"),
                                ("Cfc"),
                                ("LotNo"),
                                ("NoInLot"),
                                ("VinNo"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, checkstockissuing,
                                _ => _.PartNo,
                                _ => _.SupplierNo,
                                _ => _.Cfc,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.VinNo,
                                _ => _.MessagesError
                                );


                });

        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InvCkdStockIssuingHistorical.xlsx";
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            var allHeaders = new List<string>();
            var rowDatas = new List<JObject>();
            var exceptCols = new List<string>()
            {
                "UpdateMask",
                "ActionId",
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

                var ins = new object();
                var properties = allHeaders.Where(x => !exceptCols.Contains(x))
                                            .ToArray();
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
