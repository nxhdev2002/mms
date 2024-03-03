using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.DRM.StockPart.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.DRM.StockPart.Exporting
{
    public class InvDrmStockPartExcelExporter : NpoiExcelExporterBase, IInvDrmStockPartExcelExporter
    {

        public InvDrmStockPartExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvDrmStockPartDto> drmstockpart)
        {
            return CreateExcelPackage(
                "InventoryDRMDrmStockPart.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("DrmStockPart");
                    AddHeader(
                                sheet,
                                ("Supplier No"),
                                ("Cfc"),
                                ("Material Code"),
                                ("Material Spec"),
                                ("Part Code"),
                                ("DrmMaterial Id"),
                                ("Part No"),
                                ("Part Name"),
                                ("Part Id"),
                                ("Material Id"),
                                ("Qty"),
                                ("Working Date")
                               );
                    AddObjects(
                         sheet, drmstockpart,
                                _ => _.SupplierNo,
                                _ => _.Cfc,
                                _ => _.MaterialCode,
                                _ => _.MaterialSpec,
                                _ => _.PartCode,
                                _ => _.DrmMaterialId,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.PartId,
                                _ => _.MaterialId,
                                _ => _.Qty,
                                _ => _.WorkingDate
                                );
                });
        }

        public FileDto ExportToFileErr(List<InvDrmStockPartImportDto> listimporterr)
        {
            return CreateExcelPackage(
                "InvDirectMaterialStockPartErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("DRMStockPartListImpErr");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Supplier No"),
                                ("Cfc"),
                                ("Material Code"),
                                ("Material Spec"),
                                ("Part Code"),
                                ("Stock Qty"),
                                ("Working Date"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, listimporterr,
                                _ => _.ROW_NO,
                                _ => _.SupplierNo,
                                _ => _.Cfc,
                                _ => _.MaterialCode,
                                _ => _.MaterialSpec,
                                _ => _.PartCode,
                                _ => _.Qty,
                                _ => _.WorkingDate,
                                _ => _.ErrorDescription
                        );
                });
        }
    }
}
