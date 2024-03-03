using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Exporting;
using prod.Storage;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Exporting;
using prod.Storage;
using prod.Master.Inventory.ContainerStatus.Dto;

namespace prod.Master.Inventory.Exporting
{
    public class MstInvContainerStatusExcelExporter : NpoiExcelExporterBase, IMstInvContainerStatusExcelExporter
    {
        public MstInvContainerStatusExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvContainerStatusDto> containerstatus)
        {
            return CreateExcelPackage(
                "MasterInventoryContainerStatus.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerStatus");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Description"),
                                ("DescriptionVn"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, containerstatus,
                                _ => _.Code,
                                _ => _.Description,
                                _ => _.DescriptionVn,
                                _ => _.IsActive

                                );

                    
                });

        }
    }
}
