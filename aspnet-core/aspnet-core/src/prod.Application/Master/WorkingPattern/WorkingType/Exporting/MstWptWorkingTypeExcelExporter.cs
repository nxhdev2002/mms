using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.WorkingPattern.Dto;
using prod.Storage;

namespace prod.Master.WorkingPattern.Exporting
{
	public class MstWptWorkingTypeExcelExporter : NpoiExcelExporterBase, IMstWptWorkingTypeExcelExporter
	{
		public MstWptWorkingTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstWptWorkingTypeDto> workingtype)
		{
			return CreateExcelPackage(
				"MasterWorkingPatternWorkingType.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("WorkingType");
					AddHeader(
								sheet,
								("WorkingType"),
								("Description"),
								("IsActive")
							   );
					AddObjects(
						 sheet, workingtype,
								_ => _.WorkingType,
								_ => _.Description,
								_ => _.IsActive
								);

				
				});

		}
	}
}
