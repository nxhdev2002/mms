using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogA.Bp2.Exporting;
using prod.LogA.Bp2.Dto;
using prod.Storage;
using prod.LogA.Bp2.Dto;
namespace prod.LogA.Bp2.Exporting
{
	public class LgaBp2ProgressExcelExporter : NpoiExcelExporterBase, ILgaBp2ProgressExcelExporter
	{
		public LgaBp2ProgressExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgaBp2ProgressDto> progress)
		{
			return CreateExcelPackage(
				"LogABp2Progress.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("Progress");
					AddHeader(
								sheet,
                                ("ProcessId"),
                                ("EcarId"),
                                ("ProdLine"),
								("WorkingDate"),
								("Shift"),
								("NoInShift"),
								("NewtaktDatetime"),
								("StartDatetime"),
								("FinishDatetime"),
								("Status"),
								("IsActive")
							   );
					AddObjects(
						 sheet, progress,
								_ => _.ProcessId,
                                _ => _.EcarId,
                                _ => _.ProdLine,
								_ => _.WorkingDate,
								_ => _.Shift,
								_ => _.NoInShift,
								_ => _.NewtaktDatetime,
								_ => _.StartDatetime,
								_ => _.FinishDatetime,
								_ => _.Status,
								_ => _.IsActive
								);
				});

		}
	}
}
