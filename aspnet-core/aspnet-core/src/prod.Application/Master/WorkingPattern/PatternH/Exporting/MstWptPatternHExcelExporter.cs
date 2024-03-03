using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Storage;
using prod.Master.WorkingPattern.Dto;
using prod.Dto;

namespace prod.Master.WorkingPattern.Exporting
{
	public class MstWptPatternHExcelExporter : NpoiExcelExporterBase, IMstWptPatternHExcelExporter
	{
		public MstWptPatternHExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstWptPatternHDto> patternh)
		{
			return CreateExcelPackage(
				"MasterWorkingPatternPatternH.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PatternH");
					AddHeader(
								sheet,
								("Type"),
								("StartDate"),
								("EndDate"),
								("Description"),
								("IsActive")
							   );
					AddObjects(
						 sheet, patternh,
								_ => _.Type,
								_ => _.StartDate,
								_ => _.EndDate,
								_ => _.Description,
								_ => _.IsActive
								);
				});

		}
	}
}
