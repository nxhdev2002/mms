using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Cmm.Exporting;
using prod.Master.Cmm.Dto;
using prod.Storage;
using prod.Master.Cmm.Dto;
namespace prod.Master.Cmm.Exporting
{
	public class MstCmmTaktTimeExcelExporter : NpoiExcelExporterBase, IMstCmmTaktTimeExcelExporter
	{
		public MstCmmTaktTimeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstCmmTaktTimeDto> takttime)
		{
			return CreateExcelPackage(
				"MasterCmmTaktTime.xlsx",
				excelPackage =>
				{
				var sheet = excelPackage.CreateSheet("TaktTime");
				AddHeader(
							sheet,
							("ProdLine"),
								("GroupCd"),
								("TaktTimeSecond"),
								("TaktTime"),
								("IsActive")
							   );
			AddObjects(
				 sheet, takttime,
						_ => _.ProdLine,
						_ => _.GroupCd,
						_ => _.TaktTimeSecond,
						_ => _.TaktTime,
						_ => _.IsActive

						);
		});

        }
}
}
