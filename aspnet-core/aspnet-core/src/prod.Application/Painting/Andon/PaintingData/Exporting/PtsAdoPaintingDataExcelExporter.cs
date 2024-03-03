using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Painting.Andon.Exporting;
using prod.Painting.Andon.Dto;
using prod.Storage;
using prod.Painting.Andon.Dto;
namespace prod.Painting.Andon.Exporting
{
	public class PtsAdoPaintingDataExcelExporter : NpoiExcelExporterBase, IPtsAdoPaintingDataExcelExporter
	{
		public PtsAdoPaintingDataExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<PtsAdoPaintingDataDto> paintingdata)
		{
			return CreateExcelPackage(
				"PaintingAndonPaintingData.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PaintingData");
					AddHeader(
								sheet,
								("LifetimeNo"),
								("Model"),
								("Cfc"),
								("Grade"),
								("LotNo"),
								("NoInLot"),
								("BodyNo"),
								("Color"),
								("ProdLine"),
								("ProcessGroup"),
								("SubGroup"),
								("ProcessSeq"),
								("Filler"),
								("WorkingDate"),
								("Shift"),
								("NoInDate"),
								("ProcessCode"),
								("InfoProcess"),
								("InfoProcessNo"),
								("IsProject"),
								("IsActive")
							   );
					AddObjects(
						 sheet, paintingdata,
								_ => _.LifetimeNo,
								_ => _.Model,
								_ => _.Cfc,
								_ => _.Grade,
								_ => _.LotNo,
								_ => _.NoInLot,
								_ => _.BodyNo,
								_ => _.Color,
								_ => _.ProdLine,
								_ => _.ProcessGroup,
								_ => _.SubGroup,
								_ => _.ProcessSeq,
								_ => _.Filler,
								_ => _.WorkingDate,
								_ => _.Shift,
								_ => _.NoInDate,
								_ => _.ProcessCode,
								_ => _.InfoProcess,
								_ => _.InfoProcessNo,
								_ => _.IsProject,
								_ => _.IsActive
								);
				});

		}
	}
}