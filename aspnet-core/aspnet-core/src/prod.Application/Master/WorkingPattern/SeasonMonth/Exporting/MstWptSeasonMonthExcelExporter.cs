using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.WorkingPattern.Dto;
using prod.Storage;

namespace prod.Master.WorkingPattern.Exporting
{
	public class MstWptSeasonMonthExcelExporter : NpoiExcelExporterBase, IMstWptSeasonMonthExcelExporter
	{
		public MstWptSeasonMonthExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstWptSeasonMonthDto> seasonmonth)
		{
			return CreateExcelPackage(
				"MasterWorkingPatternSeasonMonth.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("SeasonMonth");
					AddHeader(
								sheet,
								("SeasonMonth"),
								("SeasonType"),
								("IsActive")
							   );
					AddObjects(
						 sheet, seasonmonth,
								_ => _.SeasonMonth,
								_ => _.SeasonType,
								_ => _.IsActive
								);
				});

		}
	}
}
