using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogA.Exporting;
using prod.Master.LogA.Dto;
using prod.Storage;
using prod.Master.LogA.Dto;
namespace prod.Master.LogA.Exporting
{
	public class MstLgaBp2PartListExcelExporter : NpoiExcelExporterBase, IMstLgaBp2PartListExcelExporter
	{
		public MstLgaBp2PartListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgaBp2PartListDto> bp2partlist)
		{
			return CreateExcelPackage(
				"MasterLogABp2PartList.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("Bp2PartList");
					AddHeader(
								sheet,
								("PartName"),
								("ShortName"),
								("ProdLine"),
								("PikProcess"),
								("PikSorting"),
								("DelProcess"),
								("DelSorting"),
								("IsActive")
							   );
					AddObjects(
						 sheet, bp2partlist,
								_ => _.PartName,
								_ => _.ShortName,
								_ => _.ProdLine,
								_ => _.PikProcess,
								_ => _.PikSorting,
								_ => _.DelProcess,
								_ => _.DelSorting,
								_ => _.IsActive
								);
				});

		}
	}
}
