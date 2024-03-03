using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogA.Lds.Exporting;
using prod.LogA.Lds.Dto;
using prod.Storage;
using prod.LogA.Lds.Dto;
namespace prod.LogA.Lds.Exporting
{
	public class LgaLdsLotPlanExcelExporter : NpoiExcelExporterBase, ILgaLdsLotPlanExcelExporter
	{
		public LgaLdsLotPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgaLdsLotPlanDto> lotplan)
		{
			return CreateExcelPackage(
				"LogALdsLotPlan.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("LotPlan");
					AddHeader(
								sheet,
								("ProdLine"),
								("WorkingDate"),
								("Shift"),
								("SeqLineIn"),
								("PlanStartDatetime"),
								("LotNo"),
								("LotNo2"),
								("Trip"),
								("Dolly"),
								("StartDatetime"),
								("FinishDatetime"),
								("DelaySecond"),
								("Status"),
								("IsActive")
							   );
					AddObjects(
						 sheet, lotplan,
								_ => _.ProdLine,
								_ => _.WorkingDate,
								_ => _.Shift,
								_ => _.SeqLineIn,
								_ => _.PlanStartDatetime,
								_ => _.LotNo,
								_ => _.LotNo2,
								_ => _.Trip,
								_ => _.Dolly,
								_ => _.StartDatetime,
								_ => _.FinishDatetime,
								_ => _.DelaySecond,
								_ => _.Status,
								_ => _.IsActive
								);

			
				});

		}
	}
}
