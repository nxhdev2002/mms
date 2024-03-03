using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Storage;
using prod.Master.WorkingPattern.Dto;
using prod.Dto;

namespace prod.Master.WorkingPattern.Exporting
{
	public class MstWptWorkingTimeExcelExporter : NpoiExcelExporterBase, IMstWptWorkingTimeExcelExporter
	{
		public MstWptWorkingTimeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstWptWorkingTimeDto> workingtime)
		{
			return CreateExcelPackage(
				"MasterWorkingPatternWorkingTime.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("WorkingTime");
					AddHeader(
								sheet,
								("ShiftNo"),
								("ShopId"),
								("WorkingType"),
								("StartTime"),
								("EndTime"),
								("Description"),
								("PatternHId"),
								("SeasonType"),
								("DayOfWeek"),
								("WeekWorkingDays"),
								("IsActive")
							   );
					AddObjects(
						 sheet, workingtime,
								_ => _.ShiftNo,
								_ => _.ShopId,
								_ => _.WorkingType,
								_ => _.StartTime,
								_ => _.EndTime,
								_ => _.Description,
								_ => _.PatternHId,
								_ => _.SeasonType,
								_ => _.DayOfWeek,
								_ => _.WeekWorkingDays,
								_ => _.IsActive
								);
				});

		}
	}
}
