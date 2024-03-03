using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Welding.Dto;
using prod.Storage;
using System.Collections.Generic;
namespace prod.Master.Welding.Exporting
{
	public class MstWldPunchIndicatorExcelExporter : NpoiExcelExporterBase, IMstWldPunchIndicatorExcelExporter
	{
		public MstWldPunchIndicatorExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstWldPunchIndicatorDto> punchindicator)
		{
			return CreateExcelPackage(
				"MasterWeldingPunchIndicator.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PunchIndicator");
					AddHeader(
								sheet,
								("Grade"),
								("Indicator"),
								("IsActive")
							   );
					AddObjects(
						 sheet, punchindicator,
								_ => _.Grade,
								_ => _.Indicator,
								_ => _.IsActive
								);
				});

		}
	}
}
