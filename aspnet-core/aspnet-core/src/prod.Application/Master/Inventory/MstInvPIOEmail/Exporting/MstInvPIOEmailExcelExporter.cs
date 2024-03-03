using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using prod.Storage;

namespace vovina.Master.Inventory.Exporting
{
    public class MstInvPIOEmailExcelExporter : NpoiExcelExporterBase, IMstInvPIOEmailExcelExporter
    {
        public MstInvPIOEmailExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvPIOEmailDto> pioemail)
        {
            return CreateExcelPackage(
                "MasterInventoryPIOEmail.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PIOEmail");
                    AddHeader(
                                sheet,
                                ("Subject"),
                                ("To"),
                                ("Cc"),
                                ("BodyMess"),
                                ("SupplierCd"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, pioemail,
                                _ => _.Subject,
                                _ => _.To,
                                _ => _.Cc,
                                _ => _.BodyMess,
                                _ => _.SupplierCd,
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
