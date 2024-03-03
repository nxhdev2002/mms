using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.Dto;
using prod.Storage;
using prod.Master.Inventory.Dto;
namespace prod.Master.Inventory.Exporting
{
    public class MstInvHrGlCodeCombinationExcelExporter : NpoiExcelExporterBase, IMstInvHrGlCodeCombinationExcelExporter
    {
        public MstInvHrGlCodeCombinationExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvHrGlCodeCombinationDto> hrglcodecombination)
        {
            return CreateExcelPackage(
                "MasterInventoryHrGlCodeCombination.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("HrGlCodeCombination");
                    AddHeader(
                                sheet,
                                ("AccountType"),
                                ("EnabledFlag"),
                                ("Segment1"),
                                ("Segment2"),
                                ("Segment3"),
                                ("Segment4"),
                                ("Segment5"),
                                ("Segment6"),
                                ("Concatenatedsegments"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, hrglcodecombination,
                                _ => _.AccountType,
                                _ => _.EnabledFlag,
                                _ => _.Segment1,
                                _ => _.Segment2,
                                _ => _.Segment3,
                                _ => _.Segment4,
                                _ => _.Segment5,
                                _ => _.Segment6,
                                _ => _.Concatenatedsegments,
                                _ => _.IsActive

                                );
                });

        }
    }
}