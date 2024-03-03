using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.Dto;
using prod.Storage;
using prod.Master.Inventory.Dto;
namespace prod.Master.Inventory.Exporting
{
    public class MstInvCustomsStatusExcelExporter : NpoiExcelExporterBase, IMstInvCustomsStatusExcelExporter
    {
        public MstInvCustomsStatusExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvCustomsStatusDto> customsstatus)
        {
            return CreateExcelPackage(
                "MasterInventoryCustomsStatus.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CustomsStatus");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Description"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, customsstatus,
                                _ => _.Code,
                                _ => _.Description,
                                _ => _.IsActive

                                );
                });

        }
    }
}
