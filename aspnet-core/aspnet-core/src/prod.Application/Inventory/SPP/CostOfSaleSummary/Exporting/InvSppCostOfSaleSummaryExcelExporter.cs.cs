using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.PIO.TopsseInvoice.Dto;
using prod.Inventory.PIO.TopsseInvoice.Exporting;
using prod.Inventory.SPP.CostOfSaleSummary.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.SPP.CostOfSaleSummary.Exporting
{
    public class InvSppCostOfSaleSummaryExcelExporter : NpoiExcelExporterBase, IInvSppCostOfSaleSummaryExcelExporter
    {
        public InvSppCostOfSaleSummaryExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvSppCostOfSaleSummaryDto> invsppcostofsalesummary)
        {
            return CreateExcelPackage(
                "InvSppCostOfSaleSummary.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvSppCostOfSaleSummary");
                    AddHeader(
                                sheet,
                                ("Customer No"),
                                ("Parts Qty"),
                                ("Parts Cost"),
                                ("Parts Sale Amount"),
                                ("Export Qty"),
                                ("Export Cost"),
                                ("Export Sale Amount"),
                                ("OnhandAdjustment Qty"),
                                ("OnhandAdjustment Cost"),
                                ("OnhandAdjustment Sale Amount"),
                                ("Internal Qty"),
                                ("Internal Cost"),
                                ("Internal Sale Amount"),
                                ("Others Qty"),
                                ("Others Cost"),
                                ("Others Sale Amount")
                               );
                    AddObjects(
                         sheet, invsppcostofsalesummary,
                                _ => _.CustomerNo,
                                _ => _.PartsQty,
                                _ => _.PartsCost,
                                _ => _.PartsSaleAmount,
                                _ => _.ExportQty,
                                _ => _.ExportCost,
                                _ => _.ExportSaleAmount,
                                _ => _.OnhandAdjustmentQty,
                                _ => _.OnhandAdjustmentCost,
                                _ => _.OnhandAdjustmentSaleAmount,
                                _ => _.InternalQty,
                                _ => _.InternalCost,
                                _ => _.InternalSaleAmount,
                                _ => _.OthersQty,
                                _ => _.OthersCost,
                                _ => _.OthersSaleAmount
                                );
                });
        }
    }
}
