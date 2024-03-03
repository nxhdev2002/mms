using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Lup.Exporting;
using prod.LogW.Lup.Dto;
using prod.Storage;
using prod.LogW.Lup.Dto;
namespace prod.LogW.Lup.Exporting
{
	public class LgwLupLotUpPlanExcelExporter : NpoiExcelExporterBase, ILgwLupLotUpPlanExcelExporter
	{
		public LgwLupLotUpPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgwLupLotUpPlanDto> lotupplan)
		{
			return CreateExcelPackage(
				"LogWLupLotUpPlan.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("LotUpPlan");
					AddHeader(
								sheet,
								("ProdLine"),
								("WorkingDate"),
								("Shift"),
								("NoInShift"),
								("NoInDay"),
								("LotNo"),
								("LotPartialNo"),
								("UnpackingStartDatetime"),
								("UnpackingFinishDatetime"),
								("Tpm"),
								("Remarks"),
                                ("UpCalltime"),
                                ("UnpackingActualFinishDatetime"),
                                ("UnpackingActualStartDatetime"),
                                ("UpStatus"),
                                ("MakingFinishDatetime"),
                                ("MkStatus"),
                                ("IsActive")
							   );
					AddObjects(
						 sheet, lotupplan,
								_ => _.ProdLine,
								_ => _.WorkingDate,
								_ => _.Shift,
								_ => _.NoInShift,
								_ => _.NoInDay,
								_ => _.LotNo,
								_ => _.LotPartialNo,
								_ => _.UnpackingStartDatetime,
								_ => _.UnpackingFinishDatetime,
								_ => _.Tpm,
								_ => _.Remarks,
								_ => _.UpCalltime,
                                _ => _.UnpackingActualFinishDatetime,
                                _ => _.UnpackingActualStartDatetime,
                                _ => _.UpStatus,
                                _ => _.MakingFinishDatetime,
                                _ => _.MkStatus,
                                _ => _.IsActive
								);
				});

		}
	}
}
