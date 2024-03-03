using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;
namespace prod.Master.LogW.Exporting
{
	public class MstLgwRobbingLaneExcelExporter : NpoiExcelExporterBase, IMstLgwRobbingLaneExcelExporter
	{
		public MstLgwRobbingLaneExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwRobbingLaneDto> robbinglane)
		{
			return CreateExcelPackage(
				"MasterLogWRobbingLane.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("RobbingLane");
					AddHeader(
								sheet,
								("LaneNo"),
								("LaneName"),
								("ContNo"),
								("Renban"),
								("SupplierNo"),
								("ShowOnly"),
								("IsDisabled"),
								("IsActive")
							   );
					AddObjects(
						 sheet, robbinglane,
								_ => _.LaneNo,
								_ => _.LaneName,
								_ => _.ContNo,
								_ => _.Renban,
								_ => _.SupplierNo,
								_ => _.ShowOnly,
								_ => _.IsDisabled,
								_ => _.IsActive
								);
				});

		}
	}
}
