using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Painting.Andon.Exporting;
using prod.Painting.Andon.Dto;
using prod.Storage;
using prod.Painting.Andon.Dto;
namespace prod.Painting.Andon.Exporting
{
	public class PtsAdoScanInfoExcelExporter : NpoiExcelExporterBase, IPtsAdoScanInfoExcelExporter
	{
		public PtsAdoScanInfoExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<PtsAdoScanInfoDto> scaninfo)
		{
			return CreateExcelPackage(
				"PaintingAndonScanInfo.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("ScanInfo");
					AddHeader(
								sheet,
								("ScanType"),
								("ScanValue"),
								("ScanLocation"),
								("ScanTime"),
								("ScanBy"),
								("IsProcessed")
							   );
					AddObjects(
						 sheet, scaninfo,
								_ => _.ScanType,
								_ => _.ScanValue,
								_ => _.ScanLocation,
								_ => _.ScanTime,
								_ => _.ScanBy,
								_ => _.IsProcessed
								);

				
				});

		}
	}
}
