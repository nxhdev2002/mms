using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.GPS.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.GPS.Exporting
{
    public class InvGpsReceivingExcelExporter : NpoiExcelExporterBase, IInvGpsReceivingExcelExporter
    {
        public InvGpsReceivingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvGpsReceivingDto> stockreceiving)
        {
            return CreateExcelPackage(
                "InventoryGPSReceiving.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Receiving");
                    AddHeader(
                                sheet,
								("Po No"),
								("Part No"),
                                ("Part Name"),
                                ("Uom"),
                                ("Box Qty"),
                                ("Box"),
                                ("Qty"),
                                ("Po Price"),
                                ("Lot No"),
                                ("Prod Date"),
                                ("Exp Date"),
                                ("Received Date"),
                                ("Supplier"),
                                ("ReceivedUser"),
                                ("Shop"),
                                ("Dock")
                               );
                    AddObjects(
                         sheet, stockreceiving,
						        _ => _.PoNo,
								_ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Uom,
                                _ => _.Boxqty,
                                _ => _.Box,
                                _ => _.Qty,
                                _ => _.PoPrice,
                                _ => _.LotNo,
                                _ => _.FormatProdDate,
                                _ => _.FormatExpDate,
                                _ => _.FormatReceivedDate,
                                _ => _.Supplier,
                                _ => _.UserReceives,
                                _ => _.Shop,
                                _ => _.Dock
                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }

        public FileDto ExportToFileErr(List<InvGpsReceivingImportDto> listimporterr)
        {
            return CreateExcelPackage(
                "InvGpsReceivingError.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ReceivingListImpErr");
                    AddHeader(
                                sheet,
                                ("No"),
								("Po No"),
								("Part No"),
                                ("Part Name"),
                                ("Uom"),
                                ("Box Qty"),
                                ("Box"),
                                ("Qty"),
                                ("Lot No"),
                                ("Prod Date"),
                                ("Exp Date"),
                                ("Received Date"),
                                ("Supplier"),
                                ("Error Description")
                               );
                    AddObjects(
                         sheet, listimporterr,
                                _ => _.ROW_NO,
								_ => _.PoNo,
								_ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Uom,
                                _ => _.BoxQty,
                                _ => _.Box,
                                _ => _.Qty,
                                _ => _.LotNo,
                                _ => _.ProdDate,
                                _ => _.ExpDate,
                                _ => _.ReceivedDate,
                                _ => _.Supplier,
                                _ => _.ErrorDescription
                        );
                });
        }
    }
}
