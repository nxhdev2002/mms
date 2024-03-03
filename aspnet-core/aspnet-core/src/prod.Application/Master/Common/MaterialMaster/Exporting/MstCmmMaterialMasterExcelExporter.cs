using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.Util.Collections;
using NPOI.Util;
using prod.Common;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Storage;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using prod.Master.Pio.Exporting;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.IO.Enumeration;

namespace vovina.Master.Common.Exporting
{
    public class MstCmmMaterialMasterExcelExporter : NpoiExcelExporterBase, IMstCmmMaterialMasterExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public MstCmmMaterialMasterExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<MstCmmMaterialMasterDto> materialmaster)
        {
            return CreateExcelPackage(
                "MasterCommonMaterialMaster.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("MaterialMaster");
                    AddHeader(
                                sheet,
                                ("MaterialType"),
                                ("MaterialCode"),
                                ("IndustrySector"),
                                ("Description"),
                                ("MaterialGroup"),
                                ("BaseUnitOfMeasure"),
                                ("DeletionFlag"),
                                ("Plant"),
                                ("StorageLocation"),
                                ("ProductionGroup"),
                                ("ProductionPurpose"),
                                ("ProductionType"),
                                ("ProfitCenter"),
                                ("BatchManagement"),
                                ("ReservedStock"),
                                ("LotCode"),
                                ("MrpGroup"),
                                ("MrpType"),
                                ("ProcurementType"),
                                ("SpecialProcurement"),
                                ("ProductionStorageLocation"),
                                ("RepetManufacturing"),
                                ("RemProfile"),
                                ("DoNotCost"),
                                ("VarianceKey"),
                                ("CostingLotSize"),
                                ("ProductionVersion"),
                                ("SpecialProcurementCtgView"),
                                ("ValuationCategory"),
                                ("ValuationType"),
                                ("ValuationClass"),
                                ("PriceDetermination"),
                                ("PriceControl"),
                                ("StandardPrice"),
                                ("MovingPrice"),
                                ("WithQtyStructure"),
                                ("MaterialOrigin"),
                                ("OriginGroup"),
                                ("AuthorizationGroup"),
                                ("MatSrc"),
                                ("EffectiveDateFrom"),
                                ("EffectiveDateTo"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, materialmaster,
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
                                _ => _.LotCode,
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
                                _ => _.EffectiveDateTo,
                                _ => _.IsActive

                                );

                
                });

        }
        public FileDto ExportToFileValitate(List<MstCmmMMValidationResultDto> mmvalidationresult)
        {
            return CreateExcelPackage(
                "MstCmmMMValidationResult.xlsx",
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
                         sheet, mmvalidationresult,
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
            string fileName = "MstCmmMaterialMasterHistorical.xlsx";
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
