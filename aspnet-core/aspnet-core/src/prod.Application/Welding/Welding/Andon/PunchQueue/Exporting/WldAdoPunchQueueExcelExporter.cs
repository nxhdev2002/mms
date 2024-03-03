using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Welding.Andon.Exporting;
using prod.Welding.Andon.Dto;
using prod.Storage;
using prod.Welding.Andon.Dto;
namespace prod.Welding.Andon.Exporting
{
	public class WldAdoPunchQueueExcelExporter : NpoiExcelExporterBase, IWldAdoPunchQueueExcelExporter
	{
		public WldAdoPunchQueueExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<WldAdoPunchQueueDto> punchqueue)
		{
			return CreateExcelPackage(
				"WeldingAndonPunchQueue.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PunchQueue");
					AddHeader(
								sheet,
								("BodyNo"),
								("Model"),
								("Line"),
								("LotNo"),
								("Color"),
								("ProcessGroup"),
								("ScanTime"),
								("PunchFlag"),
								("PunchIndicator"),
								("IsCall"),
								("IsCf")
							   );
					AddObjects(
						 sheet, punchqueue,
								_ => _.BodyNo,
								_ => _.Model,
								_ => _.Line,
								_ => _.LotNo,
								_ => _.Color,
								_ => _.ProcessGroup,
								_ => _.ScanTime,
								_ => _.PunchFlag,
								_ => _.PunchIndicator,
								_ => _.IsCall,
								_ => _.IsCf
								);

				
				});

		}
	}
}
