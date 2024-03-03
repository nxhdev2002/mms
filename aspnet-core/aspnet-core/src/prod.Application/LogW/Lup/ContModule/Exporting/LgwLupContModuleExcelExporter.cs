using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Lup.Exporting;
using prod.LogW.Lup.Dto;
using prod.Storage;
using prod.LogW.Lup.Dto;
namespace prod.LogW.Lup.Exporting
{
	public class LgwLupContModuleExcelExporter : NpoiExcelExporterBase, ILgwLupContModuleExcelExporter
	{
		public LgwLupContModuleExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgwLupContModuleDto> contmodule)
		{
			return CreateExcelPackage(
				"LogWLupContModule.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("ContModule");
					AddHeader(
								sheet,
								("InvoiceNo"),
								("SupplierNo"),
								("ContainerNo"),
								("Renban"),
								("LotNo"),
								("ModuleNo"),
								("Status"),
								("SortingType"),
								("SortingStatus"),
								("UpdatedSortingStatus"),
								("IsActive")
							   );
					AddObjects(
						 sheet, contmodule,
								_ => _.InvoiceNo,
								_ => _.SupplierNo,
								_ => _.ContainerNo,
								_ => _.Renban,
								_ => _.LotNo,
								_ => _.ModuleNo,
								_ => _.Status,
								_ => _.SortingType,
								_ => _.SortingStatus,
								_ => _.UpdatedSortingStatus,
								_ => _.IsActive
								);
				});

		}
	}
}
