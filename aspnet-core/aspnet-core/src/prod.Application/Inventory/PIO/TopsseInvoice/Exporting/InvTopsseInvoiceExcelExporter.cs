using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.PIO.TopsseInvoice.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.PIO.TopsseInvoice.Exporting
{
    public class InvTopsseInvoiceExcelExporter : NpoiExcelExporterBase, IInvTopsseInvoiceExcelExporter
    {
        public InvTopsseInvoiceExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvTopsseInvoiceDto> invtopsseinvoice)
        {
            return CreateExcelPackage(
                "InvPIOTopsseInvoice.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvTopsseInvoice");
                    AddHeader(
                                sheet,
                                ("Dist Fd"),
                                ("Invoice No"),
                                ("Invoice Date"),
                                ("Bill Of Lading"),
                                ("Process Date"),
                                ("Etd"),
                                ("Status")
                               );
                    AddObjects(
                         sheet, invtopsseinvoice,
                                _ => _.DistFd,
                                _ => _.InvoiceNo,
                                _ => _.InvoiceDate,
                                _ => _.BillOfLading,
                                _ => _.ProcessDate,
                                _ => _.Etd,
                                _ => _.Status
                                );
                });
        }

        public FileDto ExportToFileDetails(List<InvTopsseInvoiceDetailsDto> invtopsseinvoicedetails)
        {
            return CreateExcelPackage(
                "InvPIOTopsseInvoiceDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvTopsseInvoiceDetails");
                    AddHeader(
                                sheet,
                                ("Case No"),
                                ("Order No"),
                                ("Item No"),
                                ("Part No"),
                                ("Part Name"),
                                ("Part Qty"),
                                ("Unit Fob"),
                                ("Amount"),
                                ("Tariff Cd"),
                                ("Hs Cd"),
                                ("Receive Qty"),
                                ("Receive Date")
                               );
                    AddObjects(
                         sheet, invtopsseinvoicedetails,
                                _ => _.CaseNo,
                                _ => _.OrderNo,
                                _ => _.ItemNo,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.PartQty,
                                _ => _.UnitFob,
                                _ => _.Amount,
                                _ => _.TariffCd,
                                _ => _.HsCd,
                                _ => _.ReceiveQty,
                                _ => _.ReceiveDate
                                );
                });
        }
    }
}