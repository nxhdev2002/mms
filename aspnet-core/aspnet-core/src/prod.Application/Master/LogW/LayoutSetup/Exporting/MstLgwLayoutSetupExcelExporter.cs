using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;
namespace prod.Master.LogW.Exporting
{
	public class MstLgwLayoutSetupExcelExporter : NpoiExcelExporterBase, IMstLgwLayoutSetupExcelExporter
	{
		public MstLgwLayoutSetupExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwLayoutSetupDto> layoutsetup)
		{
			return CreateExcelPackage(
				"MasterLogWLayoutSetup.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("LayoutSetup");
					AddHeader(
								sheet,
								("Zone"),
								("SubScreenNo"),
								("ScreenArea"),
								("CellName"),
								("CellType"),
								("NumRow"),
								("ColumnName"),
								("IsDisabled"),
								("IsActive")
							   );
					AddObjects(
						 sheet, layoutsetup,
								_ => _.Zone,
								_ => _.SubScreenNo,
								_ => _.ScreenArea,
								_ => _.CellName,
								_ => _.CellType,
								_ => _.NumRow,
								_ => _.ColumnName,
								_ => _.IsDisabled,
								_ => _.IsActive
								);
				});

		}
	}
}
