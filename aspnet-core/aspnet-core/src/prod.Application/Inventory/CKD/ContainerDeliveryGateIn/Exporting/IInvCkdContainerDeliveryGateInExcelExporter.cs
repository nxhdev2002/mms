using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using prod.Inventory.CKD.Dto;
namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdContainerDeliveryGateInExcelExporter : NpoiExcelExporterBase, IInvCkdContainerDeliveryGateInExcelExporter
    {
        public InvCkdContainerDeliveryGateInExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdContainerDeliveryGateInDto> containerdeliverygatein)
        {
            return CreateExcelPackage(
                "InventoryCKDContainerDeliveryGateIn.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerDeliveryGateIn");
                    AddHeader(
                                sheet,
                                ("PlateNo"),
                                ("ContainerNo"),
                                ("Renban"),
                                ("DateIn"),
                                ("DriverName"),
                                ("Forwarder"),
                                ("TimeIn"),
                                ("TimeOut"),
                                ("CkdReqId"),
                                ("CardNo"),
                                ("Mobile"),
                                ("CallTime"),
                                ("CancelTime"),
                                ("StartTime"),
                                ("FinishTime"),
                                ("IdNo"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet,containerdeliverygatein,
                                _ => _.PlateNo,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.DateIn,
                                _ => _.DriverName,
                                _ => _.Forwarder,
                                _ => _.TimeIn,
                                _ => _.TimeOut,
                                _ => _.CkdReqId,
                                _ => _.CardNo,
                                _ => _.Mobile,
                                _ => _.CallTime,
                                _ => _.CancelTime,
                                _ => _.StartTime,
                                _ => _.FinishTime,
                                _ => _.IdNo,
                                _ => _.IsActive

                                );
                });

        }
    }
}
