using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Storage;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdPhysicalStockPeriodExcelExporter : NpoiExcelExporterBase, IInvCkdPhysicalStockPeriodExcelExporter
    {
        public InvCkdPhysicalStockPeriodExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdPhysicalStockPeriodDto> physicalstockperiod)
        {
            return CreateExcelPackage(
                "InventoryCKDPhysicalStockPeriod.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PhysicalStockPeriod");
                    AddHeader(
                                sheet,
                                ("Description"),
                                ("FromDate"),
                                ("ToDate"),
                                ("Status")
                               );
                    AddObjects(
                         sheet, physicalstockperiod,
                                _ => _.Description,
                                _ => _.FromDate,
                                _ => _.ToDate,
                                _ => _.Status
                                );

                });

        }
    }
}