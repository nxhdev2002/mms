using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inv.Dto;
using prod.Storage;
using System.Collections.Generic;
using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inv.Exporting;
using prod.Master.Inv.Dto;
using prod.Storage;
using prod.Master.Inv.Dto;
namespace prod.Master.Inv.Exporting
{
    public class MstInvGpsScreenSettingExcelExporter : NpoiExcelExporterBase, IMstInvGpsScreenSettingExcelExporter
    {
        public MstInvGpsScreenSettingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvGpsScreenSettingDto> gpsscreensetting)
        {
            return CreateExcelPackage(
                "MasterInvGpsScreenSetting.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsScreenSetting");
                    AddHeader(
                                sheet,
                                ("ScreenName"),
                                ("ScreenType"),
                                ("ScreenValue"),
                                ("Description"),
                                ("BarcodeId"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, gpsscreensetting,
                                _ => _.ScreenName,
                                _ => _.ScreenType,
                                _ => _.ScreenValue,
                                _ => _.Description,
                                _ => _.BarcodeId,
                                _ => _.IsActive

                                );
                });

        }
    }
}

