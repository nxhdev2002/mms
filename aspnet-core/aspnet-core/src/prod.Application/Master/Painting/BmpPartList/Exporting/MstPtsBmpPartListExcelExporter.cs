using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Painting.Dto;
using prod.Storage;
using System.Collections.Generic;
namespace prod.Master.Painting.Exporting
{
	public class MstPtsBmpPartListExcelExporter : NpoiExcelExporterBase, IMstPtsBmpPartListExcelExporter
	{
		public MstPtsBmpPartListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstPtsBmpPartListDto> bmppartlist)
		{
			return CreateExcelPackage(
				"MasterPaintingBmpPartList.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("BmpPartList");
					AddHeader(
								sheet,
								("Model"),
								("Cfc"),
								("Grade"),
								("BackNo"),
								("ProdLine"),
								("PartTypeCode"),
								("PartTypeId"),
								("Process"),
								("PkProcess"),
								("IsPunch"),
								("SpecialColor"),
								("SignalId"),
								("SignalCode"),
								("Remark"),
								("IsActive")
							   );
					AddObjects(
						 sheet, bmppartlist,
								_ => _.Model,
								_ => _.Cfc,
								_ => _.Grade,
								_ => _.BackNo,
								_ => _.ProdLine,
								_ => _.PartTypeCode,
								_ => _.PartTypeId,
								_ => _.Process,
								_ => _.PkProcess,
								_ => _.IsPunch,
								_ => _.SpecialColor,
								_ => _.SignalId,
								_ => _.SignalCode,
								_ => _.Remark,
								_ => _.IsActive
								);
				});

		}
	}
}
