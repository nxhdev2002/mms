using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Storage;
using System.Collections.Generic;

namespace vovina.Inventory.GPS.Exporting
{
    public class InvGpsContentListExcelExporter : NpoiExcelExporterBase, IInvGpsContentListExcelExporter
    {
        public InvGpsContentListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvGpsContentListDto> invgpscontentlist)
        {
            return CreateExcelPackage(
                "InventoryGPSInvGpsContentList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvGpsContentList");
                    AddHeader(
                                sheet,
                                ("WorkingDate"),
                                    ("Shift"),
                                    ("SupplierName"),
                                    ("SupplierCode"),
                                    ("RenbanNo"),
                                    ("PcAddress"),
                                    ("DockNo"),
                                    ("OrderNo"),
                                    ("OrderDatetime"),
                                    ("TripNo"),
                                    ("PalletBoxQty"),
                                    ("EstPackingDatetime"),
                                    ("EstArrivalDatetime"),
                                    ("ContentNo"),
                                    ("OrderId"),
                                    ("PalletSize"),
                                    ("IsPalletOnly"),
                                    ("PackagingType"),
                                    ("IsAdhocReceiving"),
                                    ("GeneratedBy"),
                                    ("UnpackStatus"),
                                    ("ModuleCd"),
                                    ("ModuleRunNo"),
                                    ("UpStartAct"),
                                    ("UpFinishAct"),
                                    ("UpScanUserId"),
                                    ("Status"),
                                    ("IsActive")
                                   );
                    AddObjects(
                         sheet, invgpscontentlist,
                                _ => _.WorkingDate,
                                _ => _.Shift,
                                _ => _.SupplierName,
                                _ => _.SupplierCode,
                                _ => _.RenbanNo,
                                _ => _.PcAddress,
                                _ => _.DockNo,
                                _ => _.OrderNo,
                                _ => _.OrderDatetime,
                                _ => _.TripNo,
                                _ => _.PalletBoxQty,
                                _ => _.EstPackingDatetime,
                                _ => _.EstArrivalDatetime,
                                _ => _.ContentNo,
                                _ => _.OrderId,
                                _ => _.PalletSize,
                                _ => _.IsPalletOnly,
                                _ => _.PackagingType,
                                _ => _.IsAdhocReceiving,
                                _ => _.GeneratedBy,
                                _ => _.UnpackStatus,
                                _ => _.ModuleCd,
                                _ => _.ModuleRunNo,
                                _ => _.UpStartAct,
                                _ => _.UpFinishAct,
                                _ => _.UpScanUserId,
                                _ => _.Status,
                                _ => _.IsActive

                                );

                    
                });

        }
    }
}
