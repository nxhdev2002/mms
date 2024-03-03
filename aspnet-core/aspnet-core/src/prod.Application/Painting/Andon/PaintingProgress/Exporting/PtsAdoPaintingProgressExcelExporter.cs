using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Painting.Andon.Exporting;
using prod.Painting.Andon.Dto;
using prod.Storage;
using prod.Painting.Andon.Dto;
namespace prod.Painting.Andon.Exporting
{
	public class PtsAdoPaintingProgressExcelExporter : NpoiExcelExporterBase, IPtsAdoPaintingProgressExcelExporter
	{
		public PtsAdoPaintingProgressExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<PtsAdoPaintingProgressDto> paintingprogress)
		{
			return CreateExcelPackage(
				"PaintingAndonPaintingProgress.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PaintingProgress");
					AddHeader(
								sheet,
								("ScanningId"),
								("BodyNo"),
								("Color"),
								("ColorOrg"),
								("ScanTypeCd"),
								("ScanLocation"),
								("ScanTime"),
								("ScanValue"),
								("Mode"),
								("ProcessGroup"),
								("ProcessSubgroup"),
								("ProcessSeq"),
								("ConveyerStatus"),
								("LastConveyerRun"),
								("TcStatus"),
								("Model"),
								("LotNo"),
								("NoInLot"),
								("SequenceNo"),
								("DefectDesc"),
								("TargetRepair"),
								("Location"),
								("DuplicateLot"),
								("WeldTransfer"),
								("RescanBodyNo"),
								("RescanLotNo"),
								("RescanMode"),
								("ErrorCd"),
								("IsRescan"),
								("IsPaintOut"),
								("IsActive")
							   );
					AddObjects(
						 sheet, paintingprogress,
								_ => _.ScanningId,
								_ => _.BodyNo,
								_ => _.Color,
								_ => _.ColorOrg,
								_ => _.ScanTypeCd,
								_ => _.ScanLocation,
								_ => _.ScanTime,
								_ => _.ScanValue,
								_ => _.Mode,
								_ => _.ProcessGroup,
								_ => _.ProcessSubgroup,
								_ => _.ProcessSeq,
								_ => _.ConveyerStatus,
								_ => _.LastConveyerRun,
								_ => _.TcStatus,
								_ => _.Model,
								_ => _.LotNo,
								_ => _.NoInLot,
								_ => _.SequenceNo,
								_ => _.DefectDesc,
								_ => _.TargetRepair,
								_ => _.Location,
								_ => _.DuplicateLot,
								_ => _.WeldTransfer,
								_ => _.RescanBodyNo,
								_ => _.RescanLotNo,
								_ => _.RescanMode,
								_ => _.ErrorCd,
								_ => _.IsRescan,
								_ => _.IsPaintOut,
								_ => _.IsActive
								);
				});

		}
	}
}
