using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Storage;
using prod.Master.WorkingPattern.Dto;
using prod.Dto;

namespace prod.Master.WorkingPattern.Exporting
{
	public class MstWptWeekExcelExporter : NpoiExcelExporterBase, IMstWptWeekExcelExporter
	{
		public MstWptWeekExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstWptWeekDto> week)
		{
			return CreateExcelPackage(
				"MasterWorkingPatternWeek.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("Week");
					AddHeader(
								sheet,
								("WorkingYear"),
								("WeekNumber"),
								("WorkingDays"),
								("IsActive")
							   );
					AddObjects(
						 sheet, week,
								_ => _.WorkingYear,
								_ => _.WeekNumber,
								_ => _.WorkingDays,
								_ => _.IsActive
								);
				});

		}
	}
}
