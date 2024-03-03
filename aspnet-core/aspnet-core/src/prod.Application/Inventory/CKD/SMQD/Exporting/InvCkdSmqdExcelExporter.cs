using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.SMQD.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.SMQD.Exporting
{
    public class InvCkdSmqdExcelExporter : NpoiExcelExporterBase, IInvCkdSmqdExcelExporter
    {
        public InvCkdSmqdExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdSmqdDto> smqd)
        {
            return CreateExcelPackage(
                "InventoryCKDSmqd.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Smqd");
                    AddHeader(
                                sheet,
                                ("Run No"),
                                ("Cfc"),
                                ("Lot No"),
                                ("Check Model"),
                                ("Supplier No"),
                                ("Part No"),
                                ("Part Name"),
                                ("Order No"),
                                ("Qty"),
                                ("Invoice"),
                                ("Received Date"),
                                ("Effect Qty"),
                                ("Reason Code"),
                                ("Order Status"),
                                ("Return Qty"),
                                ("Return Date"),
                                ("Smqd Date"),
                                ("Remark")
                               );
                    AddObjects(
                         sheet, smqd,
                                _ => _.RunNo,
                                _ => _.Cfc,
                                _ => _.LotNo,
                                _ => _.CheckModel,
                                _ => _.SupplierNo,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.OrderStatus,
                                _ => _.Qty,
                                _ => _.Invoice,
                                _ => _.ReceivedDate,
                                _ => _.EffectQty,
                                _ => _.ReasonCode,
                                _ => _.OrderStatus,
                                _ => _.ReturnQty,
                                _ => _.ReturnDate,
                                _ => _.SmqdDate,
                                _ => _.Remark
                                );
                });
        }

        public FileDto ExportToFileErr(List<InvCkdSmqdImportDto> listimporterr)
        {
            return CreateExcelPackage(
                "InvCkdSmqdImportErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvCkdSmqdListImportErr");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Smqd Date"),
                                ("Run No"),
                                ("Cfc"),
                                ("Lot No"),
                                ("Check Model"),
                                ("Supplier No"),
                                ("Part No"),
                                ("Part Name"),
                                ("Qty"),
                                ("Effect Qty"),
                                ("Reason Code"),
                                ("Order Status"),
                                ("Return Qty"),
                                ("Return Date"),
                                ("Remark"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, listimporterr,
                                _ => _.ROW_NO,
                                _ => _.SmqdDate,
                                _ => _.RunNo,
                                _ => _.Cfc,
                                _ => _.LotNo,
                                _ => _.CheckModel,
                                _ => _.SupplierNo,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Qty,
                                _ => _.EffectQty,
                                _ => _.ReasonCode,
                                _ => _.OrderStatus,
                                _ => _.ReturnQty,
                                _ => _.ReturnDate,
                                _ => _.Remark,
                                _ => _.ErrorDescription
                        );
                });
        }
    }
}
