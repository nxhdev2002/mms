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
using prod.Master.Pio.Exporting;
using System.IO;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdContainerInvoiceExcelExporter : NpoiExcelExporterBase, IInvCkdContainerInvoiceExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public InvCkdContainerInvoiceExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvCkdContainerInvoiceDto> containerinvoice)
        {
            return CreateExcelPackage(
                "InventoryCKDContainerInvoice.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerInvoice");
                    AddHeader(
                                sheet,
                                ("ContainerNo"),
                                ("Renban"),
                                ("InvoiceNo"),
                                ("SupplierNo"),
                                ("SealNo"),
                                ("BillOfLadingNo"),
                                ("BillDate"),
                                ("ContainerSize"),
                                ("PlandedvanningDate"),
                                ("ActualvanningDate"),
                                ("CdDate"),
                                ("Status"),
                                ("DateStatus"),
                                ("Fob"),
                                ("Freight"),
                                ("Insurance"),
                                ("Tax"),
                                ("Amount"),
                                ("TaxVnd"),
                                ("VatVnd")

                               );
                    AddObjects(
                         sheet, containerinvoice,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.InvoiceNo,
                                _ => _.SupplierNo,
                                _ => _.SealNo,
                                _ => _.BillOfLadingNo,
                                _ => _.BillDate,
                                _ => _.ContainerSize,
                                _ => _.PlandedvanningDate,
                                _ => _.ActualvanningDate,                        
                                _ => _.CdDate,
                                _ => _.Status,                                
                                _ => _.DateStatus,
                                _ => _.Fob,
                                _ => _.Freight,
                                _ => _.Insurance,
                                _ => _.Tax,
                                _ => _.Amount,
                                _ => _.TaxVnd,
                                _ => _.VatVnd
                                );                  
                });

        }

        public FileDto ExportCustomsToExcel(List<InvCkdContainerInvoiceCustomsDto> invckdcontainerinvoicecustoms)
        {
            return CreateExcelPackage(
               "InvCkdContainerInvoiceCustoms.xlsx",
               excelPackage =>
               {
                   var sheet = excelPackage.CreateSheet("InvoiceCustoms");
                   AddHeader(
                               sheet,
                                ("Container No"),
                                ("Renban"),
                                ("Supplier No"),
                                ("Invoice No"),
                                ("Declare Date"),
                                ("Customs Declare No"),
                                ("Taxvnd"),
                                ("Currency"),
                                ("Exchange Rate"),
                                ("BillOflading No"),
                                ("Status")
                              );
                   AddObjects(
                        sheet, invckdcontainerinvoicecustoms,
                               _ => _.ContainerNo,
                               _ => _.Renban,
                               _ => _.SupplierNo,
                               _ => _.InvoiceNo,
                               _ => _.DeclareDate,
                               _ => _.CustomsDeclareNo,
                               _ => _.Taxvnd,
                               _ => _.Currency,
                               _ => _.ExchangeRate,
                               _ => _.BillOfladingNo,
                               _ => _.Description
                               );
               });
        }

        public FileDto ExportViewCustomsToExcel(List<InvCkdContainerInvoiceViewCustomsDto> invckdcontainerinvoiceviewcustoms)
        {
            return CreateExcelPackage(
               "InvCkdContainerInvoiceViewCustoms.xlsx",
               excelPackage =>
               {
                   var sheet = excelPackage.CreateSheet("ViewCustoms");
                   AddHeader(
                               sheet,
                                ("Container No"),
                                ("Renban"),
                                ("Supplier No"),
                                ("Invoice No"),
                                ("Declare Date"),
                                ("Customs Declare No"),
                                ("Taxvnd"),
                                ("Currency"),
                                ("Exchange Rate"),
                                ("BillOflading No"),
                                ("Status")
                              );
                   AddObjects(
                        sheet, invckdcontainerinvoiceviewcustoms,
                               _ => _.ContainerNo,
                               _ => _.Renban,
                               _ => _.SupplierNo,
                               _ => _.InvoiceNo,
                               _ => _.DeclareDate,
                               _ => _.CustomsDeclareNo,
                               _ => _.Taxvnd,
                               _ => _.Currency,
                               _ => _.ExchangeRate,
                               _ => _.BillOfladingNo,
                               _ => _.Description
                               );
               });
        }

      

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InventoryCKDContainerInvoiceHistorical.xlsx";
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(InvCkdContainerInvoiceExcelExporter)} with error: {ex}");
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
