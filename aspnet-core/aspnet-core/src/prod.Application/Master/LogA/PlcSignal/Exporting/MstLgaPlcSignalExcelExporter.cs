using System.Collections.Generic;
using prod.Dto;
using prod.DataExporting.Excel.NPOI;
using prod.Storage;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA.Exporting
{
	public class MstLgaPlcSignalExcelExporter : NpoiExcelExporterBase, IMstLgaPlcSignalExcelExporter
	{
		public MstLgaPlcSignalExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgaPlcSignalDto> plcsignal)
		{
			return CreateExcelPackage(
				"MasterLogAPlcSignal.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PlcSignal");
					AddHeader(
								sheet,
								("SignalIndex"),
								("SignalPattern"),
								("ProdLine"),
								("Process"),
								("SubProcess"),
								("SignalCode"),
								("SignalDescription"),
								("IsActive")
							   );
					AddObjects(
						 sheet, plcsignal,
								_ => _.SignalIndex,
								_ => _.SignalPattern,
								_ => _.ProdLine,
								_ => _.Process,
								_ => _.SubProcess,
								_ => _.SignalCode,
								_ => _.SignalDescription,
								_ => _.IsActive
								);
				});

		}
	}
}
