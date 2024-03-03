using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.Dto;
using prod.Storage;
using prod.Master.Inventory.Dto;
namespace prod.Master.Inventory.Exporting
{
    public class MstInvPIOPartTypeExcelExporter : NpoiExcelExporterBase, IMstInvPIOPartTypeExcelExporter
    {
        public MstInvPIOPartTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvPIOPartTypeDto> pioparttype)
        {
            return CreateExcelPackage(
                "MasterInventoryPIOPartType.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PIOPartType");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Description")

                               );
                    AddObjects(
                         sheet, pioparttype,
                                _ => _.Code,
                                _ => _.Description

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
