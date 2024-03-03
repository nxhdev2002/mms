using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Painting.Andon.Exporting;
using prod.Painting.Andon.Dto;
using prod.Storage;
using prod.Painting.Andon.Dto;
namespace prod.Painting.Andon.Exporting
{
	public class PtsAdoBumperExcelExporter : NpoiExcelExporterBase, IPtsAdoBumperExcelExporter
	{
		public PtsAdoBumperExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<PtsAdoBumperDto> bumper)
		{
			return CreateExcelPackage(
				"PaintingAndonBumper.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("Bumper");
					AddHeader(
								sheet,
								("WipId"),
								("Model"),
								("Grade"),
								("BodyNo"),
								("LotNo"),
								("NoInLot"),
								("Color"),
								("BumperStatus"),
								("InitialDate"),
								("I1Date"),
								("I2Date"),
								("InlInDate"),
								("InlOutDate"),
								("PreparationDate"),
								("UnpackingDate"),
								("JigSettingDate"),
								("BoothDate"),
								("FinishedDate"),
								("ExtSeq"),
								("UnpSeq"),
								("IsActive")
							   );
					AddObjects(
						 sheet, bumper,
								_ => _.WipId,
								_ => _.Model,
								_ => _.Grade,
								_ => _.BodyNo,
								_ => _.LotNo,
								_ => _.NoInLot,
								_ => _.Color,
								_ => _.BumperStatus,
								_ => _.InitialDate,
								_ => _.I1Date,
								_ => _.I2Date,
								_ => _.InlInDate,
								_ => _.InlOutDate,
								_ => _.PreparationDate,
								_ => _.UnpackingDate,
								_ => _.JigSettingDate,
								_ => _.BoothDate,
								_ => _.FinishedDate,
								_ => _.ExtSeq,
								_ => _.UnpSeq,
								_ => _.IsActive
								);
				});

		}
	}
}
