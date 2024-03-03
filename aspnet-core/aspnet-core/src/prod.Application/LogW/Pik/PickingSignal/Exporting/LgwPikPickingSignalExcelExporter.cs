using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Pik.Exporting;
using prod.LogW.Pik.Dto;
using prod.Storage;
using prod.LogW.Pik.Dto;
namespace prod.LogW.Pik.Exporting
{
	public class LgwPikPickingSignalExcelExporter : NpoiExcelExporterBase, ILgwPikPickingSignalExcelExporter
	{
		public LgwPikPickingSignalExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgwPikPickingSignalDto> pickingsignal)
		{
			return CreateExcelPackage(
				"LogWPikPickingSignal.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PickingSignal");
					AddHeader(
								sheet,
								("PickingTabletId"),
								("TabletProcessId"),
								("PickingProgressId"),
								("FirstSignalTime"),
								("LastSignalTime"),
								("IsCompleted"),
								("IsActive")
							   );
					AddObjects(
						 sheet, pickingsignal,
								_ => _.PickingTabletId,
								_ => _.TabletProcessId,
								_ => _.PickingProgressId,
								_ => _.FirstSignalTime,
								_ => _.LastSignalTime,
								_ => _.IsCompleted,
								_ => _.IsActive
								);
				});

		}
	}
}

