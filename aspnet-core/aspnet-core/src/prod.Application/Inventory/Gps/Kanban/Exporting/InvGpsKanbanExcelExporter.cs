using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Inventory.GPS.Dto;
using prod.Storage;
using prod.Inventory.GPS.Dto;
namespace prod.Inventory.GPS.Exporting
{
    public class InvGpsKanbanExcelExporter : NpoiExcelExporterBase, IInvGpsKanbanExcelExporter
    {
        public InvGpsKanbanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvGpsKanbanDto> invgpskanban)
        {
            return CreateExcelPackage(
                "InventoryGPSInvGpsKanban.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvGpsKanban");
                    AddHeader(
                                sheet,
                                ("ContentListId"),
                                ("BackNo"),
                                ("PartNo"),
                                ("ColorSfx"),
                                ("PartName"),
                                ("BoxSize"),
                                ("BoxQty"),
                                ("PcAddress"),
                                ("WhSpsPicking"),
                                ("ActualBoxQty"),
                                ("RenbanNo"),
                                ("NoInRenban"),
                                ("PackagingType"),
                                ("ActualBoxSize"),
                                ("GeneratedBy"),
                                ("Status"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, invgpskanban,
                                _ => _.ContentListId,
                                _ => _.BackNo,
                                _ => _.PartNo,
                                _ => _.ColorSfx,
                                _ => _.PartName,
                                _ => _.BoxSize,
                                _ => _.BoxQty,
                                _ => _.PcAddress,
                                _ => _.WhSpsPicking,
                                _ => _.ActualBoxQty,
                                _ => _.RenbanNo,
                                _ => _.NoInRenban,
                                _ => _.PackagingType,
                                _ => _.ActualBoxSize,
                                _ => _.GeneratedBy,
                                _ => _.Status,
                                _ => _.IsActive

                                );

                 
                });

        }
    }
}
