using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Painting.BmpPartType.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Master.Painting.Exporting
{
	public class MstPtsBmpPartTypeExcelExporter : NpoiExcelExporterBase, IMstPtsBmpPartTypeExcelExporter
	{
		public MstPtsBmpPartTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstPtsBmpPartTypeDto> bmppartlist)
		{
			return CreateExcelPackage(
				"MasterPaintingBmpPartList.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("BmpPartList");
					AddHeader(
								sheet,
								("PartType"),
								("PartTypeName"),
								("ProdLine"),
								("Sorting"),
								("IsActive")
							   );
					AddObjects(
						 sheet, bmppartlist,
								_ => _.PartType,
								_ => _.PartTypeName,
								_ => _.ProdLine,
								_ => _.Sorting,
								_ => _.IsActive
								);
				});

		}
	}
}
