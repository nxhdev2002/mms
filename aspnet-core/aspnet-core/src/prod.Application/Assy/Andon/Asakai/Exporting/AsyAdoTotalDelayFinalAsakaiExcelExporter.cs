using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Assy.Andon.Exporting;
using prod.Assy.Andon.Dto;
using prod.Storage;
using prod.Assy.Andon.Dto;
namespace prod.Assy.Andon.Exporting
{
	public class AsyAdoTotalDelayFinalAsakaiExcelExporter : NpoiExcelExporterBase, IAsyAdoTotalDelayFinalAsakaiExcelExporter
	{
		public AsyAdoTotalDelayFinalAsakaiExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<AsyAdoTotalDelayFinalAsakaiDto> totaldelayfinalasakai)
		{
			return CreateExcelPackage(
				"AssyAndonTotalDelayFinalAsakai.xlsx",
				excelPackage =>
				{
				var sheet = excelPackage.CreateSheet("TotalDelayFinalAsakai");
				AddHeader(
							sheet,
							("BodyNo"),
								("LotNo"),
								("Color"),
								("TotalDelayLeadTime"),
								("DispatchPlanDatetime"),
								("CurrLocation"),
								("Location"),
								("WDelayWithLeadTime"),
								("TDelayWithLeadTime"),
								("ADelayWithLeadTime"),
								("IDelayWithLeadTime"),
								("FOutDelay"),
								("DelayFlag")
							   );
			AddObjects(
				 sheet, totaldelayfinalasakai,
						_ => _.BodyNo,
						_ => _.LotNo,
						_ => _.Color,
						_ => _.TotalDelayLeadTime,
						_ => _.DispatchPlanDatetime,
						_ => _.CurrLocation,
						_ => _.Location,
						_ => _.WDelayWithLeadTime,
						_ => _.TDelayWithLeadTime,
						_ => _.ADelayWithLeadTime,
						_ => _.IDelayWithLeadTime,
						_ => _.FOutDelay,
						_ => _.DelayFlag
						);
		});

        }
}
}
