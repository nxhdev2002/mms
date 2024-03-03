using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogA.Exporting;
using prod.Master.LogA.Dto;
using prod.Storage;
using prod.Master.LogA.Dto;
namespace prod.Master.LogA.Exporting
{
	public class MstLgaBp2ProcessExcelExporter : NpoiExcelExporterBase, IMstLgaBp2ProcessExcelExporter
	{
		public MstLgaBp2ProcessExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgaBp2ProcessDto> bp2process)
		{
			return CreateExcelPackage(
				"MasterLogABp2Process.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("Bp2Process");
					AddHeader(
								sheet,
								("Code"),
								("ProcessName"),
								("ProdLine"),
								("LeadTime"),
								("Sorting"),
								("ProcessType"),
								("IsActive")
							   );
					AddObjects(
						 sheet, bp2process,
								_ => _.Code,
								_ => _.ProcessName,
								_ => _.ProdLine,
								_ => _.LeadTime,
								_ => _.Sorting,
								_ => _.ProcessType,
								_ => _.IsActive
								);
				});

		}
	}
}
