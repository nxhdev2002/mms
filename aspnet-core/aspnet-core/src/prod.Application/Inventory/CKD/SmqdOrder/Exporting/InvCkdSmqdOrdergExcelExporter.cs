using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.CKD.SmqdOrder.Exporting
{
    public class InvCkdSmqdOrdergExcelExporter : NpoiExcelExporterBase, IInvCkdSmqdOrdergExcelExporter
    {
        public InvCkdSmqdOrdergExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }
        public FileDto ExportToFile(List<InvCkdSmqdOrderDto> list)
        {
            return CreateExcelPackage(
                "InventoryCKDSmqdOrder.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("SmqdOrder");
                    AddHeader(
                                sheet,
                                    "Shop",
                                    "SmqdDate",
                                    "RunNo",
                                    "Cfc",
                                    "Supplier No",
                                    "Part No",
                                    "Part Name",
                                    "Order No",
                                    "Order Qty",
                                    "Order Date",
                                    "Transport",
                                    "Reason Code",
                                    "Eta Request",
                                    "Actual EtaPort",
                                    "Eta Exp Reply",
                                    "Invoice No",
                                    "Receive Date",
                                    "Receive Qty",
                                    "Remark",
                                    "Order Type"
                                   );
                    AddObjects(
                         sheet, list,
                                _ => _.Shop,
                                _ => _.SmqdDate,
                                _ => _.RunNo,
                                _ => _.Cfc,
                                _ => _.SupplierNo,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.OrderNo,
                                _ => _.OrderQty,
                                _ => _.OrderDate,
                                _ => _.Transport,
                                _ => _.ReasonCode,
                                _ => _.EtaRequest,
                                _ => _.ActualEtaPort,
                                _ => _.EtaExpReply,
                                _ => _.InvoiceNo,
                                _ => _.ReceiveDate,
                                _ => _.ReceiveQty,
                                _ => _.Remark,
                                _ => (_.OrderType==0?"Normal":"Special")
                                );
                });
        }

        public FileDto ExportToFileErr(List<InvCkdSmqdOrderImportDto> list)
        {
            return CreateExcelPackage(
                "InventoryCKDSmqdOrderErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("SmqdOrderErr");
                    AddHeader(
                                sheet,
                                    "Guid",
                                   "Shop",
                                    "SmqdDate",
                                    "RunNo",
                                    "Cfc",
                                    "Supplier No",
                                    "Part No",
                                    "Part Name",
                                    "Order No",
                                    "Order Qty",
                                    "Order Date",
                                    "Transport",
                                    "Reason Code",
                                    "Eta Request",
                                    "Actual EtaPort",
                                    "Eta Exp Reply",
                                    "Invoice No",
                                    "Receive Date",
                                    "Receive Qty",
                                    "Remark",
                                    "Order Type",
                                    "ErrorDescription"

                                   );
                    AddObjects(
                         sheet, list,
                                _ => _.Guid,
                                 _ => _.Shop,
                                _ => _.SmqdDate,
                                _ => _.RunNo,
                                _ => _.Cfc,
                                _ => _.SupplierNo,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.OrderNo,
                                _ => _.OrderQty,
                                _ => _.OrderDate,
                                _ => _.Transport,
                                _ => _.ReasonCode,
                                _ => _.EtaRequest,
                                _ => _.ActualEtaPort,
                                _ => _.EtaExpReply,
                                _ => _.InvoiceNo,
                                _ => _.ReceiveDate,
                                _ => _.ReceiveQty,
                                _ => _.Remark,
                                _ => (_.OrderType == 0 ? "Normal" : "Special"),
                                _ => _.ErrorDescription

                                );

                });
        }
    }
}
