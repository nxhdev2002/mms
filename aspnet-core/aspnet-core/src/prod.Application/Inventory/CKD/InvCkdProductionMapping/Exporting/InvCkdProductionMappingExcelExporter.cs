using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using prod.Inventory.CKD.Dto;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using prod.Common;
using System.IO;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdProductionMappingExcelExporter : NpoiExcelExporterBase, IInvCkdProductionMappingExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public InvCkdProductionMappingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvCkdProductionMappingDto> productionmapping)
        {
            return CreateExcelPackage(
                "InventoryCKDProductionMapping.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ProductionMapping");
                    AddHeader(
                                sheet,
                                ("Plan Sequence"),
                                ("Shop"),
                                ("Model"),
                                ("Lot No"),
                                ("No In Lot"),
                                ("Grade"),
                                ("Body No"),
                                ("Date In"),
                                ("Time In"),
                                ("Use Lot No"),
                                ("Supplier No"),
                                ("PeriodId")
                               );
                    AddObjects(
                         sheet, productionmapping,
                                _ => _.PlanSequence,
                                _ => _.Shop,
                                _ => _.Model,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.Grade,
                                _ => _.BodyNo,
                                _ => _.DateIn,
                                _ => _.TimeIn,
                                _ => _.UseLotNo,
                                _ => _.SupplierNo,
                                _ => _.PeriodId
                                );

                  
                });

        }

        public FileDto ExportToFileErr(List<InvCkdProductionMappingDto> listimporterr)
        {

            return CreateExcelPackage(
                "InvCKDProdMappingListErrImport.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ProdMappingListErrImp");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Model"),
                                ("Shop"),
                                ("LotNo"),
                                ("NoInLot"),
                                ("Grade"),
                                ("BodyNo"),
                                ("DateIn"),
                                ("TimeIn"),
                                ("SupplierNo"),
                                ("UseLotNo"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, listimporterr,
                                _ => _.ROW_NO,
                                _ => _.Model,
                                _ => _.Shop,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.Grade,
                                _ => _.BodyNo,
                                _ => _.DateIn,
                                _ => _.TimeIn,
                                _ => _.SupplierNo,
                                _ => _.UseLotNo,
                                _ => _.ErrorDescription
                                );
                });
        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InventoryCkdProductionMappingHistorical.xlsx";
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
                        if (!allHeaders.Contains(prop.Name) && !exceptCols.Contains(prop.Name))
                        {
                            allHeaders.Add(prop.Name);
                        }
                    }
                }

                var properties = allHeaders.Where(x => !exceptCols.Contains(x)).ToArray();

                //Mapping data
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(InvCkdProductionMappingExcelExporter)} with error: {ex}");
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
