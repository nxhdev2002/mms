using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using prod.Inventory.CKD.CustomsDeclare.Exporting;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdCustomsDeclareExcelExporter : NpoiExcelExporterBase, IInvCkdCustomsDeclareExcelExporter
    {
        public InvCkdCustomsDeclareExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto CustomerDeclareExportToFile(List<InvCkdCustomsDeclareDto> customsdeclare)
        {
            return CreateExcelPackage(
                "InventoryCKDCustomsDeclare.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CustomsDeclare");
                    AddHeader(
                                sheet,
                                ("CustomsDeclareNo"),
                                ("DeclareDate"),
                                ("BillofladingNo"),
                                ("BillDate"),
                                ("Tax"),
                                ("Vat"),
                                ("IsFromEcus"),
                                ("Description"),
                                ("CustomsPort"),
                                ("BusinessType"),
                                ("InputDate"),
                                ("ExchangeRate"),
                                ("Forwarder"),
                                ("ActualTax"),
                                ("ActualVat"),
                                ("Sum"),
                                ("CompleteTax"),
                                ("CompleteVat"),
                                ("TaxNote")
                      

                               );
                    AddObjects(
                         sheet, customsdeclare,
                                _ => _.CustomsDeclareNo,
                                _ => _.DeclareDate,
                                _ => _.BillofladingNo,
                                _ => _.BillDate,
                                _ => _.Tax,
                                _ => _.Vat,
                                _ => _.IsFromEcus,
                                _ => _.Description,
                                _ => _.CustomsPort,
                                _ => _.BusinessType,
                                _ => _.InputDate,
                                _ => _.ExchangeRate,
                                _ => _.Forwarder,
                                _ => _.ActualTax,
                                _ => _.ActualVat,
                                _ => _.Sum,
                                _ => _.CompleteTax,
                                _ => _.CompleteVat,
                                _ => _.TaxNote
                    

                                );

                });
        }
        public FileDto PreCustomsExportToFile(List<PreCustomerDto> precustoms)
        {
            return CreateExcelPackage(
                "InventoryCKDPreCustoms.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PreCustoms");
                    AddHeader(
                                sheet,
                                ("Id"),
                                ("PreNoGroup"),
                                ("InvoiceList"),
                                ("BillofladingNo"),
                                ("Tax"),
                                ("Vat"),
                                ("Description"));
                    AddObjects(
                         sheet, precustoms,
                                _ => _.Id,
                                _ => _.PreNoGroup,
                                _ => _.InvoiceList,
                                _ => _.BillofladingNo,
                                _ => _.Tax,
                                _ => _.Vat,
                                _ => _.Description

                                );

                 
                });

        }
        public FileDto InvoiceExportToFile(List<InVoiceListDto> invoice)
        {
            return CreateExcelPackage(
                "InventoryCKDInvoice.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Invoice");
                    AddHeader(
                                sheet,
                                ("InvoiceNo"),
                                ("Fixlot"),
                                ("SupplierNo"),
                                ("PartNo"),
                                ("PartName"),
                                ("Quantity"),
                                ("Fob"),
                                ("Freight"),
                                ("Insurance"),
                                ("Thc"),
                                ("Cif"),
                                ("CeptType"),
                                ("Tax"),
                                ("Vat"),
                                ("TaxRate"),
                                ("VatRate")
                                );
                    AddObjects(
                         sheet, invoice,
                                _ => _.InvoiceNo,
                                _ => _.Fixlot,
                                _ => _.SupplierNo,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Quantity,
                                _ => _.Fob,
                                _ => _.Freight,
                                _ => _.Insurance,
                                _ => _.Thc,
                                _ => _.Cif,
                                _ => _.CeptType,
                                _ => _.Tax,
                                _ => _.Vat,
                                _ => _.TaxRate,
                                _ => _.VatRate
                                );

                });

        }
    }
}
