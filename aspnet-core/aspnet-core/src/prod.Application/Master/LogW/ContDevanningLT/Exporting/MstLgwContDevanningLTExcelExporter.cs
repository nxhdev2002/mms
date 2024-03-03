using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;
namespace prod.Master.LogW.Exporting
{
	public class MstLgwContDevanningLTExcelExporter : NpoiExcelExporterBase, IMstLgwContDevanningLTExcelExporter
	{
		public MstLgwContDevanningLTExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwContDevanningLTDto> contdevanninglt)
		{
			return CreateExcelPackage(
				"MasterLogWContDevanningLT.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("ContDevanningLT");
					AddHeader(
								sheet,
								("RenbanCode"),
								("Source"),
								("DevLeadtime"),
								("IsActive")
							   );
					AddObjects(
						 sheet, contdevanninglt,
								_ => _.RenbanCode,
								_ => _.Source,
								_ => _.DevLeadtime,
								_ => _.IsActive
								);

				
				});

		}
	}
}
