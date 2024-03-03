using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;
namespace prod.Master.LogW.Exporting
{
	public class MstLgwModelGradeExcelExporter : NpoiExcelExporterBase, IMstLgwModelGradeExcelExporter
	{
		public MstLgwModelGradeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwModelGradeDto> modelgrade)
		{
			return CreateExcelPackage(
				"MasterLogWModelGrade.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("ModelGrade");
					AddHeader(
								sheet,
								("Model"),
								("Grade"),
								("ModuleUpQty"),
								("ModuleMkQty"),
								("UpLeadtime"),
								("IsActive")
							   );
					AddObjects(
						 sheet, modelgrade,
								_ => _.Model,
								_ => _.Grade,
								_ => _.ModuleUpQty,
								_ => _.ModuleMkQty,
								_ => _.UpLeadtime,
								_ => _.IsActive
								);

		
				});

		}
	}
}
