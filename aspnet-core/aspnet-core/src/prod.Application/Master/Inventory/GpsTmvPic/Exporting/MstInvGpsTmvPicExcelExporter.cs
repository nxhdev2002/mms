using System.Collections.Generic;
using prod.Master.Inv.Exporting;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inv.Dto;
using prod.Master.Inv.Exporting;
using prod.Storage;

namespace prod.Master.Inv.Exporting
{
    public class MstInvGpsTmvPicExcelExporter : NpoiExcelExporterBase, IMstInvGpsTmvPicExcelExporter
    {
        public MstInvGpsTmvPicExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvGpsTmvPicDto> gpstmvpic)
        {
            return CreateExcelPackage(
                "MasterInvGpsTmvPic.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsTmvPic");
                    AddHeader(
                                sheet,
                                ("Suppliers"),
                                ("Pic User Account"),
                                ("Pic Name"),
                                ("Pic Telephone"),
                                ("Pic Telephone 2"),
                                ("Pic Email"),
                                ("Is Main Pic"),
                                ("Is Active")
                               );
                    AddObjects(
                         sheet, gpstmvpic,
                                _ => _.Suppliers,
                                _ => _.PicUserAccount,
                                _ => _.PicName,
                                _ => _.PicTelephone,
                                _ => _.PicTelephone2,
                                _ => _.PicEmail,
                                _ => _.IsMainPic,
                                _ => _.IsActive
                                );
                });

        }
    }
}
