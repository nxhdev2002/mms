using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.DRM.StockPartExcel.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.DRM.StockPartExcel.Exporting
{
    public class InvDrmStockPartExcelExcelExporter : NpoiExcelExporterBase, IInvDrmStockPartExcelExcelExporter
    {

        public InvDrmStockPartExcelExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFileErr(List<InvDrmStockPartExcelImportDto> listimporterr)
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
