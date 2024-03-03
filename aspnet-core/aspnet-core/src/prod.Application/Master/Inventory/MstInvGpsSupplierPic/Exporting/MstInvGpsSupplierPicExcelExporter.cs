using System.Collections.Generic;

using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inv.Dto;
using prod.Master.Inv.Exporting;
using prod.Storage;

namespace prod.Master.Inv.Exporting
{
    public class MstInvGpsSupplierPicExcelExporter : NpoiExcelExporterBase, IMstInvGpsSupplierPicExcelExporter
    {
        public MstInvGpsSupplierPicExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvGpsSupplierPicDto> gpssupplierpic)
        {
            return CreateExcelPackage(
                "MasterInvGpsSupplierPic.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsSupplierPic");
                    AddHeader(
                                sheet,
                                ("SupplierId"),
                                ("PicName"),
                                ("PicTelephone"),
                                ("PicEmail"),
                                ("IsMainPic"),
                                ("IsSendEmail"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, gpssupplierpic,
                                _ => _.SupplierId,
                                _ => _.PicName,
                                _ => _.PicTelephone,
                                _ => _.PicEmail,
                                _ => _.IsMainPic,
                                _ => _.IsSendEmail,
                                _ => _.IsActive

                                );
                });

        }
    }
}
