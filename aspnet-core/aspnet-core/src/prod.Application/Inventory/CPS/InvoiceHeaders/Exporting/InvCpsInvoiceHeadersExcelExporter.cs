using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CPS.Dto;
using prod.Inventory.CPS.Exporting;
using prod.Storage;
using System.Collections.Generic;
namespace prod.Inventory.CPS.Exporting
{
    public class InvCpsInvoiceHeadersExcelExporter : NpoiExcelExporterBase, IInvCpsInvoiceHeadersExcelExporter
    {
        public InvCpsInvoiceHeadersExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFileHeaders(List<InvCpsInvoiceHeadersGrid> invoiceheaders)
        {
            return CreateExcelPackage(
                "InventoryCPSInvoiceHeaders.xlsx",
                excelPackage =>
                {
                var sheet = excelPackage.CreateSheet("InvoiceHeaders");
                AddHeader(
                            sheet,
                                ("Invoice Symbol"),
								("Invoice No"),
								("Invoice Date"),
								("Invemtory Group"),
								("Vendor Name"),
								("Vat Invoice No"),
								("Vat Registration Num"),
								("Amount"),
								("Currency Code")
							   );
            AddObjects(
                 sheet, invoiceheaders,
                        _ => _.InvoiceSymbol,
                        _ => _.InvoiceNum,
                        _ => _.TransactionDatetime_DDMMYYYY,
                        _ => _.Productgroupname,
                        _ => _.SupplierName,
                        _ => _.VatregistrationInvoice,
                        _ => _.VatregistrationNum,
                        _ => _.FormatAmount,
                        _ => _.CurrencyCode
                        );

          
        });
        }

        public FileDto ExportToFileLines(List<InvCpsInvoiceLinesDtoGrid> invoicelines)
        {
            return CreateExcelPackage(
                "InventoryCPSInvoiceLines.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvoiceLines");
                    AddHeader(
                                sheet,
                                ("PoNumber"),
                                ("PartNo"),
                                ("PartName"),
                                ("Quantity"),
                                ("QuantityOrder"),
                                ("UnitPrice"),
                                ("Amount"),
                                ("AmountVat"),
                                ("TaxRate"),
                                ("Note")

                               );
                    AddObjects(
                         sheet, invoicelines,
                                _ => _.PoNumber,
                                _ => _.PartNo,
                                _ => _.ItemDescription,
                                _ => _.Quantity,
                                _ => _.QuantityOrder,
                                _ => _.UnitPrice,
                                _ => _.Amount,
                                _ => _.AmountVat,
                                _ => _.TaxRate,
                                _ => _.Note

                                );

                    
                });

        }
    }
}