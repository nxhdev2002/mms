using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.Invoice.Dto;
using prod.Inventory.Invoice;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Inventory.CKD.Invoice.Dto;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.Master.Pio.Exporting;
using System.IO;
using Microsoft.Extensions.Logging;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdInvoiceExcelExporter : NpoiExcelExporterBase, IInvCkdInvoiceExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        private string fileName;
        public InvCkdInvoiceExcelExporter(ITempFileCacheManager tempFileCacheManager, ILogger<InvCkdPartListExcelExporter> logger) : base(tempFileCacheManager) 
        {
            _tempFileCacheManager = tempFileCacheManager;
            _logger = logger;
        }
        public FileDto ExportToFile(List<InvCkdInvoiceDto> invoice)
        {
            return CreateExcelPackage(
                "InventoryCKDInvoice.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Invoice");
                    AddHeader(
                            sheet,
                            ("InvoiceNo"),
                            ("InvoiceDate"),
                            ("SupplierNo"),
                            ("BillNo"),
                            ("BillDate"),
                            ("ShipmentNo"),
                            ("OrdertypeCode"),
                            ("GoodstypeCode"),
                            ("InvoiceParentId"),
                            ("Fob"),
                            ("Freight"),
                            ("Insurance"),
                            ("Cif"),
                            ("Thc"),
                            ("NetWeight"),
                            ("GrossWeight"),
                            ("CeptType"),
                            ("Currency"),
                            ("Remarks"),
                            ("Quantity"),
                            ("Flag"),
                            ("Freezed"),
                            ("SourceType"),
                            ("StatusErr"),
                            ("LastOrdertype"),
                            ("Status"),
                            ("PeriodId")
                           );
                    AddObjects(
                     sheet, invoice,
                            _ => _.InvoiceNo,
                            _ => _.InvoiceDate,
                            _ => _.SupplierNo,
                            _ => _.BillNo,
                            _ => _.BillDate,
                            _ => _.ShipmentNo,
                            _ => _.OrdertypeCode,
                            _ => _.GoodstypeCode,
                            _ => _.InvoiceParentNo,
                            _ => _.Fob,
                            _ => _.FreightTotal,
                            _ => _.InsuranceTotal,
                            _ => _.Cif,
                            _ => _.ThcTotal,
                            _ => _.NetWeight,
                            _ => _.GrossWeight,
                            _ => _.CeptType,
                            _ => _.Currency,
                            _ => _.Remarks,
                            _ => _.Quantity,
                            _ => _.Flag,
                            _ => _.Freezed,
                            _ => _.SourceType,
                            _ => _.StatusErr,
                            _ => _.LastOrdertype,
                            _ => _.Description,
                            _ => _.PeriodId
                    );


                });

        }

        public FileDto ExportToFile2(List<InvCkdInvoiceDetailsDto> invoice)
        {
            return CreateExcelPackage(
                "InventoryCKDInvoice2.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Invoice");
                    AddHeader(
                                sheet,
                               ("PartNo"),
                               ("Qty"),
                               ("Fixlot"),
                               ("CaseNo"),
                               ("ModuleNo"),
                               ("ContainerNo"),
                               ("Renban"),
                               ("SupplierNo"),
                               ("Fob"),
                               ("Freight"),
                               ("Insurance"),
                               ("Fi"),
                               ("Cif"),
                               ("Thc"),
                               ("Tax"),    
                               ("Vat"),
                               ("TaxRate"),
                               ("VatRate"),
                               ("Inland"),
                               ("CeptType"),
                               ("CarfamilyCode"),
                               ("PartNetWeight"),
                               ("OrderNo"),
                               ("Firmpackingmonth"),
                               ("ReexportCode"),
                               ("Status"),
                               ("InvoiceId"),
                               ("PeriodId"),
                               ("InvoiceParentId"),
                               ("HsCode"),
                               ("PartName"),
                               ("PartnameVn"),
                               ("CarName"),
                               ("PmhistId"),
                               ("PreCustomsId"),
                               ("Ecus5TaxRate"),
                               ("Ecus5VatRate"),
                               ("Ecus5HsCode"),
                               ("DeclareType")

                               );
                    AddObjects(
                         sheet, invoice,
                                _ => _.PartNo,
                                _ => _.UsageQty,
                                _ => _.Fixlot,
                                _ => _.CaseNo,
                                _ => _.ModuleNo,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.SupplierNo,
                                _ => _.Fob,
                                _ => _.Freight,
                                _ => _.Insurance,
                                _ => _.Fi,
                                _ => _.Cif,
                                _ => _.Thc,
                                _ => _.Tax,
                                _ => _.Vat,
                                _ => _.TaxRate,
                                _ => _.VatRate,
                                _ => _.Inland,
                                _ => _.CeptType,
                                _ => _.CarfamilyCode,
                                _ => _.PartNetWeight,
                                _ => _.OrderNo,
                                _ => _.Firmpackingmonth,
                                _ => _.ReexportCode,
                                _ => _.Description,
                                _ => _.InvoiceId,
                                _ => _.PeriodId,
                                _ => _.InvoiceParentId,
                                _ => _.HsCode,
                                _ => _.PartName,
                                _ => _.PartnameVn,
                                _ => _.CarName,
                                _ => _.PmhistId,
                                _ => _.PreCustomsId,
                                _ => _.Ecus5TaxRate,
                                _ => _.Ecus5VatRate,
                                _ => _.Ecus5HsCode,
                                _ => _.DeclareType
                                );


                });

        }

        public FileDto ExportToFile3(List<InvCkdInvoiceCustomsDto> invoice)
        {
            return CreateExcelPackage(
                "InventoryCKDInvoice3.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Invoice");
                    AddHeader(
                                sheet,
                               ("InvoiceNo"),
                               ("InvoiceDate"),
                               ("Fob"),
                               ("OrdertypeCode"),
                               ("DeclareDate")
                              
                               );
                    AddObjects(
                         sheet, invoice,
                                _ => _.InvoiceNo,
                                _ => _.InvoiceDate,
                                _ => _.Fob,
                                _ => _.OrdertypeCode,
                                _ => _.DeclareDate
                                );


                });

        }
        public FileDto ExportToFileByLotNo(List<InvCkdInvoiceDetailsByLotDto> invoicedetails)
        {
            return CreateExcelPackage(
                "InvCKDInvoiceDetailsByLotNo.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvoiceDetailsByLotNo");
                    AddHeader(
                                sheet,
                               ("Part No"),
                               ("Carfamily Code"),
                               ("Supplier No"),
                               ("Lot No"),
                               ("Module No"),
                               ("Container No"),
                               ("Invoice No"),
                               ("Usage Qty"),
                               ("Part Name")
                               );
                    AddObjects(
                         sheet, invoicedetails,
                                _ => _.PartNo,
                                _ => _.CarfamilyCode,
                                _ => _.SupplierNo,
                                _ => _.LotNo,
                                _ => _.ModuleNo,
                                _ => _.ContainerNo,
                                _ => _.InvoiceNo,
                                _ => _.UsageQty,
                                _ => _.PartName
                                );
                });
        }

        public FileDto ExportToHistoricalFile(List<string> data, string tablename)
        {
            if (tablename == "InvCkdInvoiceDetails")
            {
                fileName = "InvCkdInvoiceDetailsHistorical.xlsx";
            }else if (tablename == "InvCkdInvoice")
            {
                fileName = "InvCkdInvoiceHistorical.xlsx";
            }
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(InvCkdInvoiceExcelExporter)} with error: {ex}");
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
