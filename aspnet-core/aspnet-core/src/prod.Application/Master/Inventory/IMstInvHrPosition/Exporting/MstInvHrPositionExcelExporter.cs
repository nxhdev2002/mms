using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.Dto;
using prod.Storage;
using prod.Master.Inventory.Dto;
namespace prod.Master.Inventory.Exporting
{
    public class MstInvHrPositionExcelExporter : NpoiExcelExporterBase, IMstInvHrPositionExcelExporter
    {
        public MstInvHrPositionExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvHrPositionDto> hrposition)
        {
            return CreateExcelPackage(
                "MasterInventoryHrPosition.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("HrPosition");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name"),
                                ("Description"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, hrposition,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.Description,
                                _ => _.IsActive

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
