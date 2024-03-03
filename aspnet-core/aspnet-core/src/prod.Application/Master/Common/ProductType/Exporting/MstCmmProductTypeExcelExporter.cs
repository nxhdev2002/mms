using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Storage;
using System.Collections.Generic;
namespace vovina.Master.Common.Exporting
{
	public class MstCmmProductTypeExcelExporter : NpoiExcelExporterBase, IMstCmmProductTypeExcelExporter
	{
		public MstCmmProductTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstCmmProductTypeDto> producttype)
		{
			return CreateExcelPackage(
				"MasterCommonProductType.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("ProductType");
					AddHeader(
								sheet,
								("Code"),
									("Name")
								   );
					AddObjects(
						 sheet, producttype,
								_ => _.Code,
								_ => _.Name

								);
				});

		}
	}
}
