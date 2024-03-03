using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Storage;
using prod.Master.WorkingPattern.Dto;
using prod.Dto;

namespace prod.Master.WorkingPattern.Exporting
{
	public class MstWptShopExcelExporter : NpoiExcelExporterBase, IMstWptShopExcelExporter
	{
		public MstWptShopExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstWptShopDto> shop)
		{
			return CreateExcelPackage(
				"MasterWorkingPatternShop.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("Shop");
					AddHeader(
								sheet,
								("ShopName"),
								("Description"),
								("IsActive")
							   );
					AddObjects(
						 sheet, shop,
								_ => _.ShopName,
								_ => _.Description,
								_ => _.IsActive
								);
				});

		}
	}
}
