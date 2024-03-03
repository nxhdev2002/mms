using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inv.Dto;
using prod.Storage;
using prod.Master.Inventory;

namespace prod.Master.Inv.Exporting
{
    public class MstInvGpsSupplierInfoExcelExporter : NpoiExcelExporterBase, IMstInvGpsSupplierInfoExcelExporter
    {
        public MstInvGpsSupplierInfoExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvGpsSupplierInfoDto> gpssupplierinfo)
        {
            return CreateExcelPackage(
                "MasterInvGpsSupplierInfo.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsSupplierInfo");
                    AddHeader(
                                sheet,
                                ("SupplierCode"),
                                ("SupplierPlantCode"),
                                ("SupplierName"),
                                ("Address"),
                                ("DockX"),
                                ("DockXAddress"),
                                ("DeliveryMethod"),
                                ("DeliveryFrequency"),
                                ("Cd"),
                                ("OrderDateType"),
                                ("KeihenType"),
                                ("StkConceptTmvMin"),
                                ("StkConceptTmvMax"),
                                ("StkConceptSupMMin"),
                                ("StkConceptSupMMax"),
                                ("StkConceptSupPMin"),
                                ("StkConceptSupPMax"),
                                ("TmvProductPercentage"),
                                ("PicMainId"),
                                ("DeliveryLt"),
                                ("ProductionShift"),
                                ("TcFrom"),
                                ("TcTo"),
                                ("OrderTrip"),
                                ("SupplierNameEn"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, gpssupplierinfo,
                                _ => _.SupplierCode,
                                _ => _.SupplierPlantCode,
                                _ => _.SupplierName,
                                _ => _.Address,
                                _ => _.DockX,
                                _ => _.DockXAddress,
                                _ => _.DeliveryMethod,
                                _ => _.DeliveryFrequency,
                                _ => _.Cd,
                                _ => _.OrderDateType,
                                _ => _.KeihenType,
                                _ => _.StkConceptTmvMin,
                                _ => _.StkConceptTmvMax,
                                _ => _.StkConceptSupMMin,
                                _ => _.StkConceptSupMMax,
                                _ => _.StkConceptSupPMin,
                                _ => _.StkConceptSupPMax,
                                _ => _.TmvProductPercentage,
                                _ => _.PicMainId,
                                _ => _.DeliveryLt,
                                _ => _.ProductionShift,
                                _ => _.TcFrom,
                                _ => _.TcTo,
                                _ => _.OrderTrip,
                                _ => _.SupplierNameEn,
                                _ => _.IsActive

                                );
                });
        }
    }
}
