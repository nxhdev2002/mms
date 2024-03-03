using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.DRM.Exporting;
using prod.Inventory.DRM.Dto;
using prod.Storage;
using prod.Inventory.DRM.Dto;
namespace prod.Inventory.DRM.Exporting
{
    public class InvDrmImportPlanExcelExporter : NpoiExcelExporterBase, IInvDrmImportPlanExcelExporter
    {
        public InvDrmImportPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvDrmImportPlanDto> drmimportplan)
        {
            return CreateExcelPackage(
                "InventoryDRMDrmImportPlan.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("DrmImportPlan");
                    AddHeader(
                                sheet,
                                ("SupplierNo"),
                                ("Etd"),
                                ("Eta"),
                                ("ShipmentNo"),
                                ("Cfc"),
                                ("PartCode"),
                                ("PartNo"),
                                ("PartName"),
                                ("Qty"),
                                ("PackingMonth"),
                                ("DelayEtd"),
                                ("DelayEta"),
                                ("Remark"),
                                ("Ata")
                               );
                    AddObjects(
                         sheet, drmimportplan,
                                _ => _.SupplierNo,
                                _ => _.Etd,
                                _ => _.Eta,
                                _ => _.ShipmentNo,
                                _ => _.Cfc,
                                _ => _.PartCode,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Qty,
                                _ => _.PackingMonth,
                                _ => _.DelayEtd,
                                _ => _.DelayEta,
                                _ => _.Remark,
                                _ => _.Ata
                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }

        public FileDto ExportToFileErr(List<InvDrmImportPlanImportDto> indrmimportplan_err)
        {
            return CreateExcelPackage(
                "InvDrmImportPlan List Error.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("DrmImportPlan");
                    AddHeader(
                                sheet,
                                ("Supplier No"),
                                ("Etd"),
                                ("Eta"),
                                ("Shipment No"),
                                ("Cfc"),
                                ("Part Code"),
                                ("Part No"),
                                ("Part Name"),
                                ("Qty"),
                                ("Packing Month"),
                                ("Delay Etd"),
                                ("Delay Eta"),
                                ("Ata"),
                                ("Error")
                               );
                    AddObjects(
                         sheet, indrmimportplan_err,
                                _ => _.SupplierNo,
                                _ => _.Etd,
                                _ => _.Eta,
                                _ => _.ShipmentNo,
                                _ => _.Cfc,
                                _ => _.PartCode,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Qty,
                                _ => _.PackingMonth,
                                _ => _.DelayEtd,
                                _ => _.DelayEta,
                                _ => _.Ata,
                                _ => _.ErrorDescription
                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
