using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;
namespace prod.Master.LogW.Exporting
{
	public class MstLgwPickingTabletExcelExporter : NpoiExcelExporterBase, IMstLgwPickingTabletExcelExporter
	{
		public MstLgwPickingTabletExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwPickingTabletDto> pickingtablet)
		{
			return CreateExcelPackage(
				"MasterLogWPickingTablet.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PickingTablet");
					AddHeader(
								sheet,
								("PickingTabletId"),
								("DeviceIp"),
								("ScanType"),
								("ScanName"),
								("CurrentAction"),
								("LotNo"),
								("UpTable"),
								("IsActive")
							   );
					AddObjects(
						 sheet, pickingtablet,
								_ => _.PickingTabletId,
								_ => _.DeviceIp,
								_ => _.ScanType,
								_ => _.ScanName,
								_ => _.CurrentAction,
								_ => _.LotNo,
								_ => _.UpTable,
								_ => _.IsActive
								);
				});

		}
	}
}
