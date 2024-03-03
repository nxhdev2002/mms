using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Painting.Andon.Exporting;
using prod.Painting.Andon.Dto;
using prod.Storage;
using prod.Painting.Andon.Dto;
namespace prod.Painting.Andon.Exporting
{
	public class PtsAdoLineEfficiencyExcelExporter : NpoiExcelExporterBase, IPtsAdoLineEfficiencyExcelExporter
	{
		public PtsAdoLineEfficiencyExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<PtsAdoLineEfficiencyDto> lineefficiency)
		{
			return CreateExcelPackage(
				"PaintingAndonLineEfficiency.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("LineEfficiency");
					AddHeader(
								sheet,
								("Line"),
								("Shift"),
								("WorkingDate"),
								("VolTarget"),
								("VolActual"),
								("VolBalance"),
								("StopTime"),
								("Efficiency"),
								("TaktTime"),
								("Overtime"),
								("NonProdAct"),
								("OffLine1"),
								("OffLine2"),
								("OffLine3"),
								("ShiftVolPlan"),
								("IsActive")
							   );
					AddObjects(
						 sheet, lineefficiency,
								_ => _.Line,
								_ => _.Shift,
								_ => _.WorkingDate,
								_ => _.VolTarget,
								_ => _.VolActual,
								_ => _.VolBalance,
								_ => _.StopTime,
								_ => _.Efficiency,
								_ => _.TaktTime,
								_ => _.Overtime,
								_ => _.NonProdAct,
								_ => _.OffLine1,
								_ => _.OffLine2,
								_ => _.OffLine3,
								_ => _.ShiftVolPlan,
								_ => _.IsActive
								);
				});

		}
	}
}
