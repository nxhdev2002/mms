using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inv.Exporting;
using prod.Master.Inv.Dto;
using prod.Storage;
using prod.Master.Inv.Dto;
namespace prod.Master.Inv.Exporting
{
    public class MstInvGpsCalendarExcelExporter : NpoiExcelExporterBase, IMstInvGpsCalendarExcelExporter
    {
        public MstInvGpsCalendarExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvGpsCalendarDto> gpscalendar)
        {
            return CreateExcelPackage(
                "MasterInvGpsCalendar.xlsx",
                excelPackage =>
                {
                var sheet = excelPackage.CreateSheet("GpsCalendar");
                AddHeader(
                            sheet,
                            ("SupplierCode"),
								("WorkingDate"),
								("WorkingType"),
								("WorkingStatus"),
								("IsActive")
							   );
                 AddObjects(
                 sheet, gpscalendar,
                        _ => _.SupplierCode,
                        _ => _.WorkingDate,
                        _ => _.WorkingType,
                        _ => _.WorkingStatus,
                        _ => _.IsActive

                        );
        });

        }
}
}
