using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Storage;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdPhysicalStockPartPeriodExcelExporter : NpoiExcelExporterBase, IInvCkdPhysicalStockPartPeriodExcelExporter
    {
        public InvCkdPhysicalStockPartPeriodExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdPhysicalStockPartPeriodDto> physicalstockpartperiod)
        {
            return CreateExcelPackage(
                "InventoryCKDPhysicalStockPartPeriod.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PhysicalStockPartPeriod");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartNoNormalized"),
                                ("PartName"),
                                ("PartNoNormalizedS4"),
                                ("ColorSfx"),
                                ("LotNo"),
                                ("PartListId"),
                                ("MaterialId"),
                                ("BeginQty"),
                                ("ReceiveQty"),
                                ("IssueQty"),
                                ("CalculatorQty"),
                                ("ActualQty"),
                                ("PeriodId"),
                                ("LastCalDatetime"),
                                ("Transtype"),
                                ("Remark"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, physicalstockpartperiod,
                                _ => _.PartNo,
                                _ => _.PartNoNormalized,
                                _ => _.PartName,
                                _ => _.PartNoNormalizedS4,
                                _ => _.ColorSfx,
                                _ => _.LotNo,
                                _ => _.PartListId,
                                _ => _.MaterialId,
                                _ => _.BeginQty,
                                _ => _.ReceiveQty,
                                _ => _.IssueQty,
                                _ => _.CalculatorQty,
                                _ => _.ActualQty,
                                _ => _.PeriodId,
                                _ => _.LastCalDatetime,
                                _ => _.Transtype,
                                _ => _.Remark,
                                _ => _.IsActive

                                );

                   
                });

        }
    }
}
