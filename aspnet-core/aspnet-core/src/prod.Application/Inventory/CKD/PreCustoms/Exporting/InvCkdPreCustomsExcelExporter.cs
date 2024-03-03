using System.Collections.Generic;
using prod.Inventory.CKD.Exporting;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Storage;

namespace vovina.Inventory.CKD.Exporting
{
    public class InvCkdPreCustomsExcelExporter : NpoiExcelExporterBase, IInvCkdPreCustomsExcelExporter
    {
        public InvCkdPreCustomsExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto PreCustomsExportToFile(List<InvCkdPreCustomsDto> precustoms)
        {
            return CreateExcelPackage(
                "InventoryCKDPreCustoms.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PreCustoms");
                    AddHeader(
                                sheet,
                                ("PreCustomsNo"),
                                ("BillofladingNo"),
                                ("BillDate"),
                                ("TAX"),
                                ("VAT"),
                                ("Description"));
                    AddObjects(
                         sheet, precustoms,
                                _ => _.Id,
                                _ => _.BillofladingNo,
                                _ => _.BillDate,
                                _ => _.TAX,
                                _ => _.VAT,
                                _ => _.Description                             

                                );

                  
                });

        }
        public FileDto InvoiceExportToFile(List<InvoiceListDto> invckdinvoice)
        {
            return CreateExcelPackage(
                "InventoryCKDInvoice.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PreCustoms");
                    AddHeader(
                                sheet,
                                ("SupplierNo"),
                                ("InvoiceNo"),
                                ("Fob"),
                                ("FreightTotal"),
                                ("InsuranceTotal"),
                                ("Cif"),
                                ("Currency"),
                                ("Description")
                                );
                    AddObjects(
                         sheet, invckdinvoice,
                                _ => _.SupplierNo,
                                _ => _.InvoiceNo,
                                _ => _.Fob,
                                _ => _.FreightTotal,
                                _ => _.InsuranceTotal,
                                _ => _.Cif,
                                _ => _.Currency,
                                _ => _.Description

                                );

                });

        }
        public FileDto InvoiceDetailExportToFile(List<InvoiceDetailListDto> invckinvoicedetail)
        {
            return CreateExcelPackage(
                "InventoryCKDInvoiceDetail.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PreCustoms");
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
                                ("VatRate"),
                                ("DeclareType")                               
                                );
                    AddObjects(
                         sheet, invckinvoicedetail,
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
                                _ => _.VatRate,
                                _ => _.DeclareType

                                );

                  
                });

        }
    }
}
