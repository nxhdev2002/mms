using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Pup.Exporting;
using prod.LogW.Pup.Dto;
using prod.Storage;
using prod.LogW.Pup.Dto;
namespace prod.LogW.Pup.Exporting
{
	public class LgwPupPxPUpPlanExcelExporter : NpoiExcelExporterBase, ILgwPupPxPUpPlanExcelExporter
	{
		public LgwPupPxPUpPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgwPupPxPUpPlanDto> pxpupplan)
		{
			return CreateExcelPackage(
				"LogWPupPxPUpPlan.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PxPUpPlan");
					AddHeader(
								sheet,
								("ProdLine"),
								("WorkingDate"),
								("Shift"),
								("NoInShift"),
								("SeqLineIn"),
								("CaseNo"),
								("SupplierNo"),
								("UpTable"),
								("IsNoPxpData"),
								("UnpackingStartDatetime"),
								("UnpackingFinishDatetime"),
								("IsActive")
							   );
					AddObjects(
						 sheet, pxpupplan,
								_ => _.ProdLine,
								_ => _.WorkingDate,
								_ => _.Shift,
								_ => _.NoInShift,
								_ => _.SeqLineIn,
								_ => _.CaseNo,
								_ => _.SupplierNo,
								_ => _.UpTable,
								_ => _.IsNoPxpData,
								_ => _.UnpackingStartDatetime,
								_ => _.UnpackingFinishDatetime,
								_ => _.IsActive
								);
				});

		}
	}
}
