
using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;

namespace prod.Master.LogW.Exporting
{
	public class MstLgwLayoutExcelExporter : NpoiExcelExporterBase, IMstLgwLayoutExcelExporter
	{
		public MstLgwLayoutExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwLayoutDto> layout)
		{
			return CreateExcelPackage(
				"MasterLogWLayout.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("Layout");
					AddHeader(
								sheet,
								("ZoneCd"),
								("AreaCd"),
								("RowId"),
								("ColumnId"),
								("RowName"),
								("ColumnName"),
								("LocationCd"),
								("LocationName"),
								("LocationTitle"),
								("IsDisabled"),
								("IsActive")
							   );
					AddObjects(
						 sheet, layout,
								_ => _.ZoneCd,
								_ => _.AreaCd,
								_ => _.RowId,
								_ => _.ColumnId,
								_ => _.RowName,
								_ => _.ColumnName,
								_ => _.LocationCd,
								_ => _.LocationName,
								_ => _.LocationTitle,
								_ => _.IsDisabled,
								_ => _.IsActive
								);
				});

		}
	}
}
