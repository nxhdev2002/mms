using Abp.Application.Services;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.IHP.Exporting;
using prod.Inventory.PIO.PartListInl.Dto;
using prod.Master.Pio.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.PartListInl.Export
{
    public class InvPioPartListInlExcelExporter : NpoiExcelExporterBase, IInvPioPartListInlExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;

        public InvPioPartListInlExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        public FileDto ExportValidateToFile(List<ValidatePartListDto> invckdpartlistvalidate)
        {
            return CreateExcelPackage(
                "InventoryPIOPartListInlValidate.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartList");
                    AddHeader(
                                sheet,
                                ("PartListId"),
                                ("ErrorDescription"),
                                ("PartNo"),
                                ("PartNoNormalizedS4"),
                                ("PartName"),
                 
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
                                ("EndRun")
                   
                               );
                    AddObjects(
                         sheet, invckdpartlistvalidate,
                             _ => _.PartListId,
                             _ => _.ErrorDescription,
                             _ => _.PartNo,
                             _ => _.PartNoNormalizedS4,
                             _ => _.PartName,
                  
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
                             _ => _.EndRun
       
                                );


                });

        }


        public FileDto ExportToFileErr(List<ImportPioPartListDto> errpartlist)
        {
            return CreateExcelPackage(
                "ListErrorImportPioPartListInl.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartListError");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Model"),
                                ("Shop"),
                                ("IdLine"),
                                ("PartNo"),
                                ("PartName"),
                                ("SupplierNo"),
                                ("SupplierCd"),
                                ("BodyColor"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, errpartlist,
                                _ => _.ROW_NO,
                                _ => _.Model,
                                _ => _.Shop,
                                _ => _.IdLine,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.SupplierNo,
                                _ => _.SupplierCd,
                                _ => _.BodyColor,
                                _ => _.ErrorDescription
                                );


                });

        }
        public FileDto ExportToFileLotErr(List<ImportPioPartListDto> errpartlist)
        {
            return CreateExcelPackage(
                "ListErrorImportPioPartLotInl.xlsx",
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

        public FileDto ExportToHistoricalFile(List<string> data, string tableName)
        {
            string fileName = "";
            if (tableName == "InvPioPartListInl")
            {
                fileName = "InventoryCkdPartListInlHistorical.xlsx";
            }
            else if (tableName == "InvPioPartGradeInl")
            {
                fileName = "InventoryCkdPartGradeInlHistorical.xlsx";
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
