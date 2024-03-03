using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.ContainerDeliveryType;
using prod.Master.Inventory.ContainerDeliveryType.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Master.Inventory.Exporting
{
    public class MstInvContainerDeliveryTypeExcelExporter : NpoiExcelExporterBase, IMstInvContainerDeliveryTypeExcelExporter
    {
        public MstInvContainerDeliveryTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvContainerDeliveryTypeDto> containerdeliverytype)
        {
            return CreateExcelPackage(
                "MasterInventoryContainerDeliveryType.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerDeliveryType");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Description"),
                                ("IsActive"));
                    AddObjects(
                         sheet, containerdeliverytype,
                                _ => _.Code,
                                _ => _.Description,
                                _ => _.IsActive

                                );

                   
                });

        }
    }
}
