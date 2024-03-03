using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.SPP.InvoiceDetails.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.SPP.InvoiceDetails.Exporting
{
    public class InvSppInvoiceDetailsExcelExporter : NpoiExcelExporterBase, IInvSppInvoiceDetailsExcelExporter
    {
        public InvSppInvoiceDetailsExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvSppInvoiceDetailsDto> InvSppInvoiceDetails)
        {
            return CreateExcelPackage(
                "InvSppInvoiceDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvSppInvoiceDetails");
                    AddHeader(
                                sheet,
                                    ("InvoiceId"),
                                    ("InvoiceNo"),
                                    ("PartNo"),
                                    ("Supplier"),
                                    ("Fob"),
                                    ("Freight"),
                                    ("Insurance"),
                                    ("Lc"),
                                    ("Cif"),
                                    ("Vat"),
                                    ("InLand"),
                                    ("Tax"),
                                    ("InvoiceQty"),
                                    ("ReceicedQty"),
                                    ("RejectQty"),
                                    ("PartId"),
                                    ("Type"),
                                    ("CaseNo"),
                                    ("Month"),
                                    ("Year"),
                                    ("Stock"),
                                    ("FobVn"),
                                    ("PONo"),
                                    ("LcVn"),
                                    ("CifVn"),
                                    ("VatVn"),
                                    ("InlandVn"),
                                    ("TaxVn"),
                                    ("ItemNo"),
                                    ("FreightVn"),
                                    ("InsuranceVn"),
                                    ("ReceiveDate"),
                                    ("ProcessDate"),
                                    ("CustomerNo"),
                                    ("IsInternal")
                               );
                    AddObjects(
                         sheet, InvSppInvoiceDetails,
                                    _ => _.InvoiceId,
                                    _ => _.InvoiceNo,
                                    _ => _.PartNo,
                                    _ => _.Supplier,
                                    _ => _.Fob,
                                    _ => _.Freight,
                                    _ => _.Insurance,
                                    _ => _.Lc,
                                    _ => _.Cif,
                                    _ => _.Vat,
                                    _ => _.InLand,
                                    _ => _.Tax,
                                    _ => _.InvoiceQty,
                                    _ => _.ReceicedQty,
                                    _ => _.RejectQty,
                                    _ => _.PartId,
                                    _ => _.Type,
                                    _ => _.CaseNo,
                                    _ => _.Month,
                                    _ => _.Year,
                                    _ => _.Stock,
                                    _ => _.FobVn,
                                    _ => _.PONo,
                                    _ => _.LcVn,
                                    _ => _.CifVn,
                                    _ => _.VatVn,
                                    _ => _.InlandVn,
                                    _ => _.TaxVn,
                                    _ => _.ItemNo,
                                    _ => _.FreightVn,
                                    _ => _.InsuranceVn,
                                    _ => _.ReceiveDate,
                                    _ => _.ProcessDate,
                                    _ => _.CustomerNo,
                                    _ => _.IsInternal
                                );
                });
        }
    }
}
