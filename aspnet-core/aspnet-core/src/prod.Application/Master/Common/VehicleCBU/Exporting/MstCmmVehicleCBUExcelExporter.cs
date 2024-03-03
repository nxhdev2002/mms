using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Tls.Crypto;
using prod.Common;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.VehicleCBU.Dto;
using prod.Master.Pio.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace prod.Master.Common.VehicleCBU.Exporting
{

    public class MstCmmVehicleCBUExcelExporter : NpoiExcelExporterBase, IMstCmmVehicleCBUExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public MstCmmVehicleCBUExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) 
        {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<MstCmmVehicleCBUDto> vehiclecbu)
        {
            return CreateExcelPackage(
                "MasterCmmVehicleCBU.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("VehicleCBU");
                    AddHeader(
                                sheet,
                                ("Vehicle Type"),
                                ("Model"),
                                ("Marketing Code"),
                                ("Production Code"),
                                ("Katashiki"),
                                ("Material Type"),
                                ("Material Code"),
                                ("Industry Sector"),
                                ("Description"),
                                ("Material Group"),
                                ("Base Unit Of Measure"),
                                ("Deletion Flag"),
                                ("Plant"),
                                ("Storage Location"),
                                ("Production Group"),
                                ("Production Purpose"),
                                ("Production Type"),
                                ("Profit Center"),
                                ("Batch Management"),
                                ("Reserved Stock"),
                                ("Lot Code M"),
                                ("Mrp Group"),
                                ("Mrp Type"),
                                ("Procurement Type"),
                                ("Special Procurement"),
                                ("Production Storage Location"),
                                ("Repet Manufacturing"),
                                ("Rem Profile"),
                                ("Do Not Cost"),
                                ("Variance Key"),
                                ("Costing Lot Size"),
                                ("Production Version"),
                                ("Special Procurement Ctg View"),
                                ("Valuation Category"),
                                ("Valuation Type"),
                                ("Valuation Class"),
                                ("Price Determination"),
                                ("Price Control"),
                                ("Standard Price"),
                                ("Moving Price"),
                                ("With Qty Structure"),
                                ("Material Origin"),
                                ("Origin Group"),
                                ("Authorization Group"),
                                ("Mat Src"),
                                ("Effective Date From"),
                                ("Effective Date To")
                               );
                    AddObjects(
                         sheet, vehiclecbu,
                                _ => _.VehicleType,
                                _ => _.Model,
                                _ => _.MarketingCode,
                                _ => _.ProductionCode,
                                _ => _.Katashiki,
                                _ => _.MaterialType,
                                _ => _.MaterialCode,
                                _ => _.IndustrySector,
                                _ => _.Description,
                                _ => _.MaterialGroup,
                                _ => _.BaseUnitOfMeasure,
                                _ => _.DeletionFlag,
                                _ => _.Plant,
                                _ => _.StorageLocation,
                                _ => _.ProductionGroup,
                                _ => _.ProductionPurpose,
                                _ => _.ProductionType,
                                _ => _.ProfitCenter,
                                _ => _.BatchManagement,
                                _ => _.ReservedStock,
                                _ => _.LotCodeM,
                                _ => _.MrpGroup,
                                _ => _.MrpType,
                                _ => _.ProcurementType,
                                _ => _.SpecialProcurement,
                                _ => _.ProductionStorageLocation,
                                _ => _.RepetManufacturing,
                                _ => _.RemProfile,
                                _ => _.DoNotCost,
                                _ => _.VarianceKey,
                                _ => _.CostingLotSize,
                                _ => _.ProductionVersion,
                                _ => _.SpecialProcurementCtgView,
                                _ => _.ValuationCategory,
                                _ => _.ValuationType,
                                _ => _.ValuationClass,
                                _ => _.PriceDetermination,
                                _ => _.PriceControl,
                                _ => _.StandardPrice,
                                _ => _.MovingPrice,
                                _ => _.WithQtyStructure,
                                _ => _.MaterialOrigin,
                                _ => _.OriginGroup,
                                _ => _.AuthorizationGroup,
                                _ => _.MatSrc,
                                _ => _.EffectiveDateFrom,
                                _ => _.EffectiveDateTo
                                );
                });
        }

        public FileDto ExportToFileColor(List<MstCmmVehicleCBUColorDto> mstcmmvehiclecbucolor)
        {
            return CreateExcelPackage(
                "MasterCmmVehicleCBUColor.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("VehicleCBUColor");
                    AddHeader(
                                sheet,
                                ("Ext Color"),
                                ("Int Color")
                               );
                    AddObjects(
                         sheet, mstcmmvehiclecbucolor,
                                _ => _.ExtColor,
                                _ => _.IntColor
                                );
                });
        }
        public FileDto ExportToFileValitate(List<MstCmmVehicleCBUColorValidationResultDto> vehicleCBUvalidationresult)
        {
            return CreateExcelPackage(
                "<MstCmmVehicleCBUColorValidationResult.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ValidationResult");
                    AddHeader(
                                sheet,
                                ("MateriaId"),
                                ("MaterialCode"),
                                ("MaterialName"),
                                ("MaterialGroup"),
                                ("ValuationClass"),
                                ("RuleId"),
                                ("RuleCode"),
                                ("RuleDescription"),
                                ("RuleItem"),
                                ("Option"),
                                ("ResultField"),
                                ("ExpectedResult"),
                                ("ActualResult"),
                                ("LastValidationDatetime"),
                                ("Lastvalidationby"),
                                ("LastValidationId"),
                                ("Status"),
                                ("ErrorMessage")

                               );
                    AddObjects(
                         sheet, vehicleCBUvalidationresult,
                                _ => _.MateriaId,
                                _ => _.MaterialCode,
                                _ => _.MaterialName,
                                _ => _.MaterialGroup,
                                _ => _.ValuationClass,
                                _ => _.RuleId,
                                _ => _.RuleCode,
                                _ => _.RuleDescription,
                                _ => _.RuleItem,
                                _ => _.Option,
                                _ => _.ResultField,
                                _ => _.ExpectedResult,
                                _ => _.ActualResult,
                                _ => _.LastValidationDatetime,
                                _ => _.Lastvalidationby,
                                _ => _.LastValidationId,
                                _ => _.Status,
                                _ => _.ErrorMessage
                                );


                });

        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "MstCmmVehicleCBUHistorical.xlsx";
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(MstCmmVehicleCBUExcelExporter)} with error: {ex}");
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
