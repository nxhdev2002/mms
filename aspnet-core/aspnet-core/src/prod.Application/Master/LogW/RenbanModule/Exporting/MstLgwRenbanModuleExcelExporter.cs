using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;
namespace prod.Master.LogW.Exporting
{
	public class MstLgwRenbanModuleExcelExporter : NpoiExcelExporterBase, IMstLgwRenbanModuleExcelExporter
	{
		public MstLgwRenbanModuleExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwRenbanModuleDto> renbanmodule)
		{
			return CreateExcelPackage(
				"MasterLogWRenbanModule.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("RenbanModule");
					AddHeader(
								sheet,
								("Renban"),
								("CaseNo"),
								("SupplierNo"),
								("MinModule"),
								("MaxModule"),
								("ModuleCapacity"),
								("ModuleType"),
								("ModuleSize"),
								("SortingType"),
								("MinMod"),
								("MaxMod"),
								("MonitorVisualize"),
								("CaseOrder"),
								("CaseType"),
								("ProdLine"),
								("Model"),
								("Cfc"),
								("WhLoc"),
								("IsUsePxpData"),
								("UpLeadtime"),
								("Remark"),
								("IsActive")
							   );
					AddObjects(
						 sheet, renbanmodule,
								_ => _.Renban,
								_ => _.CaseNo,
								_ => _.SupplierNo,
								_ => _.MinModule,
								_ => _.MaxModule,
								_ => _.ModuleCapacity,
								_ => _.ModuleType,
								_ => _.ModuleSize,
								_ => _.SortingType,
								_ => _.MinMod,
								_ => _.MaxMod,
								_ => _.MonitorVisualize,
								_ => _.CaseOrder,
								_ => _.CaseType,
								_ => _.ProdLine,
								_ => _.Model,
								_ => _.Cfc,
								_ => _.WhLoc,
								_ => _.IsUsePxpData,
								_ => _.UpLeadtime,
								_=>_.Remark,
								_ => _.IsActive
								);
				});

		}
	}
}
