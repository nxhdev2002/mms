using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.SPP.Shipping.Dto;
using prod.Inventory.SPP.Shipping.Exporting;
using prod.Inventory.SPP.Stock.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.SPP.Stock.Exporting
{
    public class InvSppStockExcelExporter : NpoiExcelExporterBase, IInvSppStockExcelExporter
    {
        public InvSppStockExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvSppStockDto> InvSppStock)
        {
            return CreateExcelPackage(
                "InvSppStock.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvSppStock");
                    AddHeader(
                                sheet,
                                    ("PartNo"),
                                    ("Month"),
                                    ("Year"),
                                    ("Amount"),
                                    ("Qty"),
                                    ("PreAmount"),
                                    ("PreQty"),
                                    ("Price"),
                                    ("PrePrice"),
                                    ("Warehouse"),
                                    ("PriceVn"),
                                    ("PrePriceVn"),
                                    ("AmountVn"),
                                    ("PreAmountVn"),
                                    ("InQty"),
                                    ("InAmount"),
                                    ("InPrice"),
                                    ("OutQty"),
                                    ("OutAmount"),
                                    ("OutPrice"),
                                    ("InAmountVn"),
                                    ("InPriceVn"),
                                    ("OutAmountVn"),
                                    ("OutPriceVn")
                               );
                    AddObjects(
                         sheet, InvSppStock,
                                    _ => _.PartNo,
                                    _ => _.Month,
                                    _ => _.Year,
                                    _ => _.Amount,
                                    _ => _.Qty,
                                    _ => _.PreAmount,
                                    _ => _.PreQty,
                                    _ => _.Price,
                                    _ => _.PrePrice,
                                    _ => _.Warehouse,
                                    _ => _.PriceVn,
                                    _ => _.PrePriceVn,
                                    _ => _.AmountVn,
                                    _ => _.PreAmountVn,
                                    _ => _.InQty,
                                    _ => _.InAmount,
                                    _ => _.InPrice,
                                    _ => _.OutQty,
                                    _ => _.OutAmount,
                                    _ => _.OutPrice,
                                    _ => _.InAmountVn,
                                    _ => _.InPriceVn,
                                    _ => _.OutAmountVn,
                                    _ => _.OutPriceVn
                                );
                });
        }
    }
}
