using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Frame.Andon.Exporting;
using prod.Frame.Andon.Dto;
using prod.Storage;
using prod.Frame.Andon.Dto;
namespace prod.Frame.Andon.Exporting
{
	public class FrmAdoFramePlanExcelExporter : NpoiExcelExporterBase, IFrmAdoFramePlanExcelExporter
	{
		public FrmAdoFramePlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<FrmAdoFramePlanDto> frameplan)
		{
			return CreateExcelPackage(
				"FrameAndonFramePlan.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("FramePlan");
					AddHeader(
								sheet,
								("No"),
								("Model"),
								("LotNo"),
								("NoInLot"),
								("BodyNo"),
								("Color"),
								("VinNo"),
								("FrameId"),
								("Status"),
								("PlanMonth"),
								("PlanDate"),
								("Grade"),
								("Version"),
								("IsActive")
							   );
					AddObjects(
						 sheet, frameplan,
								_ => _.No,
								_ => _.Model,
								_ => _.LotNo,
								_ => _.NoInLot,
								_ => _.BodyNo,
								_ => _.Color,
								_ => _.VinNo,
								_ => _.FrameId,
								_ => _.Status,
								_ => _.PlanMonth,
								_ => _.PlanDate,
								_ => _.Grade,
								_ => _.Version,
								_ => _.IsActive
								);
				});

		}
	}
}
