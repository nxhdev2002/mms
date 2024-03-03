using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Charts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Tls.Crypto;
using prod.Common;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.GradeColor.Dto;
using prod.Master.Pio.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Common.LotCodeGrade.Exporting
{
    public class MstCmmLotCodeGradeExcelExporter : NpoiExcelExporterBase, IMstCmmLotCodeGradeExcelExporter
    {
        public readonly ITempFileCacheManager _tempFileCacheManager;
        public readonly ILogger _logger;
        public MstCmmLotCodeGradeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<MstCmmLotCodeGradeTDto> mstcmmlotcodegrade)
        {
            return CreateExcelPackage(
                "MstCommonLotCodeGrade.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("LotCodeGrade");
                    AddHeader(
                                sheet,
                                ("Id"),
                                ("Model"),
                                ("LotCode"),
                                ("Cfc"),
                                ("Grade"),
                                ("GradeName"),
                                ("ModelCode"),
                                ("ModelVin"),
                                ("IdLine"),
                                ("Spec200"),
                                ("SsNo"),
                                ("Katashiki"),
                                ("VehNameCd"),
                                ("KatashikiCtl"),
                                ("MarLotCode"),           
                                ("TestNo"),
                                ("VehicleId"),
                                ("ProdSfx"),
                                ("SalesSfx"),
                                ("Brand"),
                                ("CarSeries"),
                                ("TransmissionType"),
                                ("EngineType"),
                                ("FuelType"),
                                ("GoshiCar"),
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
                                ("LotCodeM"),
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
                         sheet, mstcmmlotcodegrade,
                                _ => _.Id,
                                _ => _.Model,
                                _ => _.LotCode,
                                _ => _.Cfc,
                                _ => _.Grade,
                                _ => _.GradeName,
                                _ => _.ModelCode,
                                _ => _.ModelVin,
                                _ => _.IdLine,
                                _ => _.Spec200,
                                _ => _.SsNo,
                                _ => _.Katashiki,
                                _ => _.VehNameCd,
                                _ => _.KatashikiCtl,
                                _ => _.MarLotCode,
                                _ => _.TestNo,
                                _ => _.VehicleId,
                                _ => _.ProdSfx,
                                _ => _.SalesSfx,
                                _ => _.Brand,
                                _ => _.CarSeries,
                                _ => _.TransmissionType,
                                _ => _.EngineType,
                                _ => _.FuelType,
                                _ => _.GoshiCar,
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
                                _ => _.EffectiveDateTo,
                                _ => _.IsActive
                               
                                );
                });

        }
    public FileDto ExportToHistoricalFile(List<string> data)
    {
        string fileName = "InventoryMstCmmGradeColorHistorical.xlsx";
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
            _logger.LogError($"[EXCEPTION] While exporting {nameof(MstCmmLotCodeGradeExcelExporter)} with error: {ex}");
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
