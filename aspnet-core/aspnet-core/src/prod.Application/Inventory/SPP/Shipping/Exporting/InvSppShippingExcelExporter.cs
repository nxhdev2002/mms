using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.SPP.Shipping.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.SPP.Shipping.Exporting
{
    public class InvSppShippingExcelExporter : NpoiExcelExporterBase, IInvSppShippingExcelExporter
    {
        public InvSppShippingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvSppShippingDto> InvSppShipping)
        {
            return CreateExcelPackage(
                "InvSppShipping.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvSppShipping");
                    AddHeader(
                                sheet,
                                    ("PartNo"),
                                    ("PartName"),
                                    ("CustomerOrderNo"),
                                    ("CustomerNo"),
                                    ("InvoiceNo"),
                                    ("CustomerType"),
                                    ("Exporter"),
                                    ("OrderQty"),
                                    ("AllcocatedQty"),
                                    ("SalePrice"),
                                    ("Remark"),
                                    ("SaleAmount"),
                                    ("Month"),
                                    ("Year"),
                                    ("Stock"),
                                    ("CorrectionCd"),
                                    ("RouteNo"),
                                    ("Price"),
                                    ("PriceVn"),
                                    ("SalePriceUsd"),
                                    ("SaleAmountUsd"),
                                    ("ItemNo"),
                                    ("ProcessDate"),
                                    ("SalesPriceCd"),
                                    ("CorrectionReason"),
                                    ("FrCd")
                               );
                    AddObjects(
                         sheet, InvSppShipping,
                                    _ => _.PartNo,
                                    _ => _.PartName,
                                    _ => _.CustomerOrderNo,
                                    _ => _.CustomerNo,
                                    _ => _.InvoiceNo,
                                    _ => _.CustomerType,
                                    _ => _.Exporter,
                                    _ => _.OrderQty,
                                    _ => _.AllcocatedQty,
                                    _ => _.SalePrice,
                                    _ => _.Remark,
                                    _ => _.SaleAmount,
                                    _ => _.Month,
                                    _ => _.Year,
                                    _ => _.Stock,
                                    _ => _.CorrectionCd,
                                    _ => _.RouteNo,
                                    _ => _.Price,
                                    _ => _.PriceVn,
                                    _ => _.SalePriceUsd,
                                    _ => _.SaleAmountUsd,
                                    _ => _.ItemNo,
                                    _ => _.ProcessDate,
                                    _ => _.SalesPriceCd,
                                    _ => _.CorrectionReason,
                                    _ => _.FrCd
                                );
                });
        }
    }
}
