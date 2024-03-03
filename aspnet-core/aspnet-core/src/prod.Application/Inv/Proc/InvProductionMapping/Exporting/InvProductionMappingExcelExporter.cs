using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inv.Dmr.Dto;
using prod.Inv.Dmr.Exporting;
using prod.Inv.Proc.Exporting;
using prod.Inv.Proc.Dto;
using prod.Storage;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.Master.Pio.Exporting;
using System.IO;
using Microsoft.Extensions.Logging;

namespace prod.Inv.Proc.Exporting
{
    public class InvProductionMappingExcelExporter : NpoiExcelExporterBase, IInvProductionMappingExcelExporter
    {
        public readonly ITempFileCacheManager _tempFileCacheManager;
        public readonly ILogger _logger;
        public InvProductionMappingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvProductionMappingDto> invimportprocductionmapping)
        {
            return CreateExcelPackage(
                "InvImportProductionMapping.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvImportProductionMapping");
                    AddHeader(
                                sheet,
                                ("PlanSequence"),
                                ("Shop"),
                                ("Model"),
                                ("LotNo"),
                                ("NoInLot"),
                                ("Grade"),
                                ("BodyNo"),
                                ("DateIn"),
                                ("TimeIn"),
                                ("UseLotNo"),
                                ("SupplierNo"),
                                ("PartId"),
                                ("Quantity"),
                                ("Cost"),
                                ("Cif"),
                                ("Fob"),
                                ("Freight"),
                                ("Insurance"),
                                ("Thc"),
                                ("Tax"),
                                ("InLand"),
                                ("Amount"),
                                ("PeriodId"),
                                ("CostVn"),
                                ("CifVn"),
                                ("FobVn"),
                                ("FreightVn"),
                                ("InsuranceVn"),
                                ("ThcVn"),
                                ("TaxVn"),
                                ("InlandVn"),
                                ("AmountVn"),
                                ("WipId"),
                                ("InStockId"),
                                ("MappingId")
                               );
                    AddObjects(
                         sheet, invimportprocductionmapping,
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
                                _ => _.PartId,
                                _ => _.Quantity,
                                _ => _.Cost,
                                _ => _.Cif,
                                _ => _.Fob,
                                _ => _.Freight,
                                _ => _.Insurance,
                                _ => _.Thc,
                                _ => _.Tax,
                                _ => _.InLand,
                                _ => _.Amount,
                                _ => _.PeriodId,
                                _ => _.CostVn,
                                _ => _.CifVn,
                                _ => _.FobVn,
                                _ => _.FreightVn,
                                _ => _.InsuranceVn,
                                _ => _.ThcVn,
                                _ => _.TaxVn,
                                _ => _.InlandVn,
                                _ => _.AmountVn,
                                _ => _.WipId,
                                _ => _.InStockId,
                                _ => _.MappingId
                                );
                });

        }
        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InvProductionMappingHistory.xlsx";
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

