using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Painting.Andon.Exporting;
using prod.Painting.Andon.Dto;
using prod.Storage;
using prod.Painting.Andon.Dto;
namespace prod.Painting.Andon.Exporting
{
	public class PtsAdoLineEfficiencyDetailsExcelExporter : NpoiExcelExporterBase, IPtsAdoLineEfficiencyDetailsExcelExporter
	{
		public PtsAdoLineEfficiencyDetailsExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<PtsAdoLineEfficiencyDetailsDto> lineefficiencydetails)
		{
			return CreateExcelPackage(
				"PaintingAndonLineEfficiencyDetails.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("LineEfficiencyDetails");
					AddHeader(
								sheet,
								("Line"),
								("VolActual"),
								("LineStopTime"),
								("LineEfficiency"),
								("WorkingDate"),
								("Shift"),
								("Status")
							   );
					AddObjects(
						 sheet, lineefficiencydetails,
								_ => _.Line,
								_ => _.VolActual,
								_ => _.LineStopTime,
								_ => _.LineEfficiency,
								_ => _.WorkingDate,
								_ => _.Shift,
								_ => _.Status
								);

				
				});

		}
	}
}
