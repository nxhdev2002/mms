using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogA.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Master.LogA.Exporting
{
	public class MstLgaSpsRackExcelExporter : NpoiExcelExporterBase, IMstLgaSpsRackExcelExporter
	{
		public MstLgaSpsRackExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgaSpsRackDto> spsrack)
		{
			return CreateExcelPackage(
				"MasterLogASpsRack.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("SpsRack");
					AddHeader(
								sheet,
								("Code"),
								("Address"),
								("Ordering"),
								("IsActive")
							   );
					AddObjects(
						 sheet, spsrack,
								_ => _.Code,
								_ => _.Address,
								_ => _.Ordering,
								_ => _.IsActive
								);

				
				});

		}
	}
}
