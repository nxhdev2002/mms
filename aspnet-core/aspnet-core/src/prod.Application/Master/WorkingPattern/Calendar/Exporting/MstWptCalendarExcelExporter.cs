using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.WorkingPattern.Dto;
using prod.Storage;
using System.Collections.Generic;
using prod.Master.WorkingPattern.Exporting;

namespace prod.MasterWorkingPattern.Exporting
{
    public class MstWptCalendarExcelExporter : NpoiExcelExporterBase, IMstWptCalendarExcelExporter
    {
        public MstWptCalendarExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstWptCalendarDto> calendar)
        {
            return CreateExcelPackage(
                                 "MasterWorkingPatternCalendar.xlsx",
                                 excelPackage =>
                                 {
                                     var sheet = excelPackage.CreateSheet("Calendar");
                                     AddHeader(
                                               sheet,
                                                     ("WorkingDate"),
                                                     ("WorkingType"),
                                                     ("WorkingStatus"),
                                                     ("SeasonType"),
                                                     ("DayOfWeek"),
                                                     ("WeekNumber"),
                                                     ("WeekWorkingDays"),
                                                     ("IsActive")

                                               );
                                     AddObjects(
                                         sheet, calendar,
                                         _ => _.WorkingDate,
                                         _ => _.WorkingType,
                                          _ => _.WorkingStatus,
                                         _ => _.SeasonType,
                                         _ => _.DayOfWeek,
                                           _ => _.WeekNumber,
                                         _ => _.WeekWorkingDays,
                                         _ => _.IsActive
                                         );
                                 });

        }
    }
}
