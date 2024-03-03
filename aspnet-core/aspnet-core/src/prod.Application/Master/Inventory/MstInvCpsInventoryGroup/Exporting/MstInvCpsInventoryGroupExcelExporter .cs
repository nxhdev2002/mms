using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using prod.Storage;
using System.Collections.Generic;

namespace vovina.Master.Inventory.Exporting
{
    public class MstInvCpsInventoryGroupExcelExporter : NpoiExcelExporterBase, IMstInvCpsInventoryGroupExcelExporter
    {
        public MstInvCpsInventoryGroupExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvCpsInventoryGroupDto> cpsinventorygroup)
        {
            return CreateExcelPackage(
                "MasterInventoryCpsInventoryGroup.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CpsInventoryGroup");
                    AddHeader(
                                sheet,
                                ("Productgroupcode"),
                                    ("Productgroupname"),
                                    ("IsActive")
                                   );
                    AddObjects(
                         sheet, cpsinventorygroup,
                                _ => _.Productgroupcode,
                                _ => _.Productgroupname,
                                _ => _.IsActive

                                );
                });

        }
    }
}
