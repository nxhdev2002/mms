using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;
namespace prod.Master.LogW.Exporting
{
	public class MstLgwUnpackingPartExcelExporter : NpoiExcelExporterBase, IMstLgwUnpackingPartExcelExporter
	{
		public MstLgwUnpackingPartExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwUnpackingPartDto> unpackingpart)
		{
			return CreateExcelPackage(
				"MasterLogWUnpackingPart.xlsx",
				excelPackage =>
				{
				var sheet = excelPackage.CreateSheet("UnpackingPart");
				AddHeader(
							sheet,
							("Cfc"),
								("Model"),
								("BackNo"),
								("PartNo"),
								("PartName"),
								("ModuleCode"),
								("IsActive")
							   );
			AddObjects(
				 sheet, unpackingpart,
						_ => _.Cfc,
						_ => _.Model,
						_ => _.BackNo,
						_ => _.PartNo,
						_ => _.PartName,
						_ => _.ModuleCode,
						_ => _.IsActive
						);
		});

        }
}
}
