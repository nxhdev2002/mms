using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Plan.Ccr.Exporting;
using prod.Plan.Ccr.Dto;
using prod.Storage;
using prod.Plan.Ccr.Dto;
namespace prod.Plan.Ccr.Exporting
{
	public class PlnCcrProductionPlanExcelExporter : NpoiExcelExporterBase, IPlnCcrProductionPlanExcelExporter
	{
		public PlnCcrProductionPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<PlnCcrProductionPlanDto> productionplan)
		{
			return CreateExcelPackage(
				"PlanCcrProductionPlan.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("ProductionPlan");
					AddHeader(
								sheet,
								("PlanSequence"),
								("Shop"),
								("Model"),
								("LotNo"),
								("NoInLot"),
								("Grade"),
								("Body"),
								("DateIn"),
								("TimeIn"),
								("DateTimeIn"),
								("SupplierNo"),
								("UseLotNo"),
								("SupplierNo2"),
								("UseLotNo2"),
								("UseNoInLot")
							   );
					AddObjects(
						 sheet, productionplan,
								_ => _.PlanSequence,
								_ => _.Shop,
								_ => _.Model,
								_ => _.LotNo,
								_ => _.NoInLot,
								_ => _.Grade,
								_ => _.Body,
								_ => _.DateIn,
								_ => _.TimeIn,
								_ => _.DateTimeIn,
								_ => _.SupplierNo,
								_ => _.UseLotNo,
								_ => _.SupplierNo2,
								_ => _.UseLotNo2,
								_ => _.UseNoInLot
								);

				
				});

		}
	}
}
