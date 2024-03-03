using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;
namespace prod.Master.LogW.Exporting
{
	public class MstLgwPlcSignalExcelExporter : NpoiExcelExporterBase, IMstLgwPlcSignalExcelExporter
	{
		public MstLgwPlcSignalExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwPlcSignalDto> plcsignal)
		{
			return CreateExcelPackage(
				"MasterLogWPlcSignal.xlsx",
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

