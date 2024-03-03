using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.Dto;
using prod.Storage;
using prod.Master.Inv.Exporting;

namespace prod.Master.Inventory.Exporting
{
    public class MstInvSupplierListExcelExporter : NpoiExcelExporterBase, IMstInvSupplierListExcelExporter
    {
        public MstInvSupplierListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvSupplierListDto> supplierlist)
        {
            return CreateExcelPackage(
                "MasterInventorySupplierList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("SupplierList");
                    AddHeader(
                                sheet,
                                ("SupperlierNo"),
                                ("SupperlierName"),
                                ("Remarks"),
                                ("SupplierType"),
                                ("SupplierNameVn"),
                                ("Exporter")
                               );
                    AddObjects(
                         sheet, supplierlist,
                                _ => _.SupplierNo,
                                _ => _.SupplierName,
                                _ => _.Remarks,
                                _ => _.SupplierType,
                                _ => _.SupplierNameVn,
                                _ => _.Exporter
                                );
                });

        }
    }
}


