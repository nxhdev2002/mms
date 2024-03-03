using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Storage;
using prod.Master.WorkingPattern.Dto;
using prod.Dto;

namespace prod.Master.WorkingPattern.Exporting
{
	public class MstWptDailyWorkingTimeExcelExporter : NpoiExcelExporterBase, IMstWptDailyWorkingTimeExcelExporter
	{
		public MstWptDailyWorkingTimeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstWptDailyWorkingTimeDto> dailyworkingtime)
		{
			return CreateExcelPackage(
				"MasterWorkingPatternDailyWorkingTime.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("DailyWorkingTime");
					AddHeader(
								sheet,
								("ShiftNo"),
								("ShiftName"),
								("ShopId"),
								("WorkingDate"),
								("StartTime"),
								("EndTime"),
								("WorkingType"),
								("Description"),
								("FromTime"),
								("ToTime"),
								("IsManual"),
								("IsActive")
							   );
					AddObjects(
						 sheet, dailyworkingtime,
								_ => _.ShiftNo,
								_ => _.ShiftName,
								_ => _.ShopId,
								_ => _.WorkingDate,
								_ => _.StartTime,
								_ => _.EndTime,
								_ => _.WorkingType,
								_ => _.Description,
								_ => _.FromTime,
								_ => _.ToTime,
								_ => _.IsManual,
								_ => _.IsActive
								);
				});

		}
	}
}
