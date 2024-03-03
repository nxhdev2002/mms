using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Pik.Exporting;
using prod.LogW.Pik.Dto;
using prod.Storage;
using prod.LogW.Pik.Dto;
namespace prod.LogW.Pik.Exporting
{
	public class LgwPikPickingProgressExcelExporter : NpoiExcelExporterBase, ILgwPikPickingProgressExcelExporter
	{
		public LgwPikPickingProgressExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgwPikPickingProgressDto> pickingprogress)
		{
			return CreateExcelPackage(
				"LogWPikPickingProgress.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PickingProgress");
					AddHeader(
								sheet,
								("PickingTabletId"),
								("ProdLine"),
                                ("BodyNo"),
                                ("LotNo"),
                                ("ProcessCode"),
								("ProcessGroup"),
								("SeqNo"),
								("WorkingDate"),
								("Shift"),
								("TaktStartTime"),
								("StartTime"),
								("FinishTime"),
								("IsActive")
							   );
					AddObjects(
						 sheet, pickingprogress,
								_ => _.PickingTabletId,
								_ => _.ProdLine,
                                _ => _.BodyNo,
                                _ => _.LotNo,
                                _ => _.ProcessCode,
								_ => _.ProcessGroup,
								_ => _.SeqNo,
								_ => _.WorkingDate,
								_ => _.Shift,
								_ => _.TaktStartTime,
								_ => _.StartTime,
								_ => _.FinishTime,
								_ => _.IsActive
								);
				});

		}
	}
}
