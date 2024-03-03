using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Painting.Andon.Exporting;
using prod.Painting.Andon.Dto;
using prod.Storage;
using prod.Painting.Andon.Dto;
namespace prod.Painting.Andon.Exporting
{
	public class PtsAdoTotalDelayExcelExporter : NpoiExcelExporterBase, IPtsAdoTotalDelayExcelExporter
	{
		public PtsAdoTotalDelayExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<PtsAdoTotalDelayDto> totaldelay)
		{
			return CreateExcelPackage(
				"PaintingAndonTotalDelay.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("TotalDelay");
					AddHeader(
								sheet,
								("WipId"),
								("ProgressId"),
								("BodyNo"),
								("LotNo"),
								("Color"),
								("Mode"),
								("TargetRepair"),
								("StartRepair"),
								("Location"),
								("AInPlanDate"),
								("EdInAct"),
								("RepairIn"),
								("Leadtime"),
								("LeadtimePlus"),
								("Etd"),
								("RecoatIn"),
								("IsActive")
							   );
					AddObjects(
						 sheet, totaldelay,
								_ => _.WipId,
								_ => _.ProgressId,
								_ => _.BodyNo,
								_ => _.LotNo,
								_ => _.Color,
								_ => _.Mode,
								_ => _.TargetRepair,
								_ => _.StartRepair,
								_ => _.Location,
								_ => _.AInPlanDate,
								_ => _.EdInAct,
								_ => _.RepairIn,
								_ => _.Leadtime,
								_ => _.LeadtimePlus,
								_ => _.Etd,
								_ => _.RecoatIn,
								_ => _.IsActive
								);

				
				});

		}
	}
}
