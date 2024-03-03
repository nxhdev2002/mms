using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Mwh.Exporting;
using prod.LogW.Mwh.Dto;
using prod.Storage;
using prod.LogW.Mwh.Dto;
namespace prod.LogW.Mwh.Exporting
{
	public class LgwMwhRobbingLaneExcelExporter : NpoiExcelExporterBase, ILgwMwhRobbingLaneExcelExporter
	{
		public LgwMwhRobbingLaneExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgwMwhRobbingLaneDto> robbinglane)
		{
			return CreateExcelPackage(
				"LogWMwhRobbingLane.xlsx",
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
								_ => _.IsActive
								);
				});

		}
	}
}
