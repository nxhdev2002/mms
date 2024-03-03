using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.DRM.Dto;
using prod.LogA.Pcs.Stock.Dto;
using prod.Master.Pio.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace prod.Inventory.DRM.Exporting
{
    public class InvDrmPartListExcelExporter : NpoiExcelExporterBase, IInvDrmPartListExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public InvDrmPartListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvDrmPartListDto> drmpartlist)
        {
            return CreateExcelPackage(
                "InventoryDRMPartList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("DrmPartList");
                    AddHeader(
                            sheet,
                            ("Supplier Type"),
                            ("Supplier Cd"),
                            ("Cfc"),
                            ("Material Code"),
                            ("Material Spec"),
                            ("Part Spec"),
                            ("Part Size"),
                            ("Box Qty"),
                            ("Part Code"),
                            ("First Day Product"),
                            ("Last Day Product"),
                            ("Asset Id"),
                            ("MainAssetNumBer"),
                            ("AssetSubNumber"),
                            ("WBS"),
                            ("CostCenter"),
                            ("Responsible Cost Center"),
                            ("CostOfAsset")
                            //("Sourcing"),
                            //("Cutting"),
                            //("Packing"),
                            //("Sheet Weight"),
                            //("Yiled Ration")
                               );
                    AddObjects(
                    sheet, drmpartlist,
                        _ => _.SupplierType,
                        _ => _.SupplierCd,
                        _ => _.Cfc,
                        _ => _.MaterialCode,
                        _ => _.MaterialSpec,
                        _ => _.PartSpec,
                        _ => _.PartSize,
                        _ => _.BoxQty,
                        _ => _.PartCode,
                        _ => _.FirstDayProduct,
                        _ => _.LastDayProduct,
                        _ => _.AssetId,
                        _ => _.MainAssetNumber,
                        _ => _.AssetSubNumber,
                        _ => _.WBS,
                        _ => _.CostCenter,
                        _ => _.ResponsibleCostCenter,
                        _ => _.CostOfAsset
                        );
                });
        }

        public FileDto ExportToFileErr(List<InvDrmIhpPartImportDto> list_drm_import_err,
                                       List<InvDrmIhpPartImportDto> list_ihp_import_err)
        {

            return CreateExcelPackage(
                "InvDirectMaterialErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("DRMImpErr");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Supplier Type"),
                                ("Supplier Cd"),
                                ("Cfc"),
                                ("Material Code"),
                                ("Material Spec"),
                                ("Box Qty"),
                                ("Part Code"),
                                ("Part Size"),
                                ("Part Spec"),
                                ("First Day Product"),
                                ("Last Day Product"),
                                ("Error Description")
                               );
                    AddObjects(
                         sheet, list_drm_import_err,
                                _ => _.ROW_NO,
                                _ => _.SupplierType,
                                _ => _.SupplierDrm,
                                _ => _.Cfc,
                                _ => _.MaterialCode,
                                _ => _.MaterialSpec,
                                _ => _.BoxQty,
                                _ => _.PartCode,
                                _ => _.Size,
                                _ => _.Spec,
                                _ => _.FirstDayProduct,
                                _ => _.LastDayProduct,
                                _ => _.ErrorDescription
                        );

                    var sheet1 = excelPackage.CreateSheet("IHPImpErr");
                    AddHeader(
                                sheet1,
                                ("No"),
                                ("Cfc"),
                                ("Part No"),
                                ("Part Name"),                              
                                ("Sourcing"),
                                ("Cutting"),
                                ("Packing"),
                                ("Sheet Weight"),
                                ("Yiled Ration"),
                                ("Error Description")
                               );
                    AddObjects(
                         sheet1, list_ihp_import_err,
                                _ => _.ROW_NO,
                                _ => _.CfcIhp,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Sourcing,
                                _ => _.Cutting,
                                _ => _.Packing,
                                _ => _.SheetWeight,
                                _ => _.YiledRation,
                                _ => _.ErrorDescription
                        );
                });
        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InventoryDrmPartListHistorical.xlsx";
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
                        if (prop.Name == "Action")
                        {

                            switch (prop.Value.ToString())
                            {
                                case "1":
                                    prop.Value = "Xóa";
                                    break;
                                case "2":
                                    prop.Value = "Tạo mới";
                                    break;
                                case "3":
                                    prop.Value = "Trước Update";
                                    break;
                                case "4":
                                    prop.Value = "Sau Update";
                                    break;
                            }
                        }
                        if (!allHeaders.Contains(prop.Name) && !exceptCols.Contains(prop.Name))
                        {
                            allHeaders.Add(prop.Name);
                        }
                    }
                }

                var ins = new object();
                var properties = allHeaders.Where(x => !exceptCols.Contains(x))
                                            .ToArray();

                Commons.FillHistoriesExcel(rowDatas, workSheet, 1, 0, properties, properties);
                xlWorkBook.Save(tempFile);
                using (var obj_stream = new MemoryStream(File.ReadAllBytes(tempFile)))
                {
                    _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[EXCEPTION] While exporting {nameof(InvDrmPartListExcelExporter)} with error: {ex}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            return file;
        }
    }
}
