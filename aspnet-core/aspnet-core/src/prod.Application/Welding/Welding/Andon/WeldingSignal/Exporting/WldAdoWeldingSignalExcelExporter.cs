using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Welding.Andon.Exporting;
using prod.Welding.Andon.Dto;
using prod.Storage;
using prod.Welding.Andon.Dto;
namespace prod.Welding.Andon.Exporting
{
	public class WldAdoWeldingSignalExcelExporter : NpoiExcelExporterBase, IWldAdoWeldingSignalExcelExporter
	{
		public WldAdoWeldingSignalExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<WldAdoWeldingSignalDto> weldingsignal)
		{
			return CreateExcelPackage(
				"WeldingAndonWeldingSignal.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("WeldingSignal");
					AddHeader(
								sheet,
								("ProdLine"),
								("Process"),
								("SignalType"),
								("SignalBy"),
								("SignalDate"),
								("IsActive")
							   );
					AddObjects(
						 sheet, weldingsignal,
								_ => _.ProdLine,
								_ => _.Process,
								_ => _.SignalType,
								_ => _.SignalBy,
								_ => _.SignalDate,
								_ => _.IsActive
								);
				});

		}
	}
}
