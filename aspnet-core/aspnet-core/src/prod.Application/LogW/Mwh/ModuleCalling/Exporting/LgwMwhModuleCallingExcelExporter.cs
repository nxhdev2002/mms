using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Mwh.Exporting;
using prod.LogW.Mwh.Dto;
using prod.Storage;
using prod.LogW.Mwh.Dto;
namespace prod.LogW.Mwh.Exporting
{
	public class LgwMwhModuleCallingExcelExporter : NpoiExcelExporterBase, ILgwMwhModuleCallingExcelExporter
	{
		public LgwMwhModuleCallingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgwMwhModuleCallingDto> modulecalling)
		{
			return CreateExcelPackage(
				"LogWMwhModuleCalling.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("ModuleCalling");
					AddHeader(
								sheet,
								("Renban"),
								("CaseNo"),
								("SupplierNo"),
								("CallTime"),
								("CalledModuleNo"),
								("CaseType"),
								("IsActive")
							   );
					AddObjects(
						 sheet, modulecalling,
								_ => _.Renban,
								_ => _.CaseNo,
								_ => _.SupplierNo,
								_ => _.CallTime,
								_ => _.CalledModuleNo,
								_ => _.CaseType,
								_ => _.IsActive
								);
				});

		}
	}
}
