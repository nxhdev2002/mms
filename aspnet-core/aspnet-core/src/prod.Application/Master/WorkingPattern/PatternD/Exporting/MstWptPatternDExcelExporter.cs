using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.WorkingPattern.Dto;
using prod.Storage;
using System.Collections.Generic;
namespace prod.Master.WorkingPattern.Exporting
{
    public class MstWptPatternDExcelExporter : NpoiExcelExporterBase, IMstWptPatternDExcelExporter
	{
		public MstWptPatternDExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstWptPatternDDto> patternd)
		{
			return CreateExcelPackage(
				"MasterWorkingPatternPatternD.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PatternD");
					AddHeader(
								sheet,
								("PatternHId"),
								("ShiftNo"),
								("ShiftName"),
								("StartTime"),
								("EndTime"),
								("DayOfWeek"),
								("SeasonType"),
								("IsActive")
							   );
					AddObjects(
						 sheet, patternd,
								_ => _.PatternHId,
								_ => _.ShiftNo,
								_ => _.ShiftName,
								_ => _.StartTime,
								_ => _.EndTime,
								_ => _.DayOfWeek,
								_ => _.SeasonType,
								_ => _.IsActive
								);
				});

		}
	}
}
