using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;
namespace prod.Master.LogW.Exporting
{
	public class MstLgwEciPartExcelExporter : NpoiExcelExporterBase, IMstLgwEciPartExcelExporter
	{
		public MstLgwEciPartExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwEciPartDto> ecipart)
		{
			return CreateExcelPackage(
				"MasterLogWEciPart.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("EciPart");
					AddHeader(
								sheet,
								("ModuleNo"),
								("PartNo"),
								("SupplierNo"),
								("ModuleNoEci"),
								("PartNoEci"),
								("SupplierNoEci"),
								("StartEciSeq"),
								("StartEciRenban"),
								("StartEciModule"),
								("IsActive")
							   );
					AddObjects(
						 sheet, ecipart,
								_ => _.ModuleNo,
								_ => _.PartNo,
								_ => _.SupplierNo,
								_ => _.ModuleNoEci,
								_ => _.PartNoEci,
								_ => _.SupplierNoEci,
								_ => _.StartEciSeq,
								_ => _.StartEciRenban,
								_ => _.StartEciModule,
								_ => _.IsActive
								);
				});

		}
	}
}