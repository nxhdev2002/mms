using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Pup.Exporting;
using prod.LogW.Pup.Dto;
using prod.Storage;
using prod.LogW.Pup.Dto;
namespace prod.LogW.Pup.Exporting
{
	public class LgwPupPxPUpPlanBaseExcelExporter : NpoiExcelExporterBase, ILgwPupPxPUpPlanBaseExcelExporter
	{
		public LgwPupPxPUpPlanBaseExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgwPupPxPUpPlanBaseDto> pxpupplanbase)
		{
			return CreateExcelPackage(
				"LogWPupPxPUpPlanBase.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PxPUpPlanBase");
					AddHeader(
								sheet,
								("WorkingDate"),
								("ProdLine"),
								("Shift1"),
								("Shift2"),
								("Shift3"),
								("IsActive")
							   );
					AddObjects(
						 sheet, pxpupplanbase,
								_ => _.WorkingDate,
								_ => _.ProdLine,
								_ => _.Shift1,
								_ => _.Shift2,
								_ => _.Shift3,
								_ => _.IsActive
								);
				});

		}
	}
}
