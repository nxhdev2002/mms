using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Frame.Andon.Exporting;
using prod.Frame.Andon.Dto;
using prod.Storage;
using prod.Frame.Andon.Dto;
namespace prod.Frame.Andon.Exporting
{
	public class FrmAdoFrameProgressExcelExporter : NpoiExcelExporterBase, IFrmAdoFrameProgressExcelExporter
	{
		public FrmAdoFrameProgressExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<FrmAdoFrameProgressDto> frameprogress)
		{
			return CreateExcelPackage(
				"FrameAndonFrameProgress.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("FrameProgress");
					AddHeader(
								sheet,
								("ScanningId"),
								("BodyNo"),
								("Color"),
								("ScanTypeCd"),
								("ScanLocation"),
								("ScanTime"),
								("ScanValue"),
								("Mode"),
								("ProcessGroup"),
								("ProcessSubgroup"),
								("VinNo"),
								("FrameNo"),
								("Model"),
								("Grade"),
								("LotNo"),
								("NoInLot"),
								("SequenceNo"),
								("Location"),
								("AndonTransfer"),
								("RescanBodyNo"),
								("RescanLotNo"),
								("RescanMode"),
								("ErrorCd"),
								("IsRescan"),
								("IsActive")
							   );
					AddObjects(
						 sheet, frameprogress,
								_ => _.ScanningId,
								_ => _.BodyNo,
								_ => _.Color,
								_ => _.ScanTypeCd,
								_ => _.ScanLocation,
								_ => _.ScanTime,
								_ => _.ScanValue,
								_ => _.Mode,
								_ => _.ProcessGroup,
								_ => _.ProcessSubgroup,
								_ => _.VinNo,
								_ => _.FrameNo,
								_ => _.Model,
								_ => _.Grade,
								_ => _.LotNo,
								_ => _.NoInLot,
								_ => _.SequenceNo,
								_ => _.Location,
								_ => _.AndonTransfer,
								_ => _.RescanBodyNo,
								_ => _.RescanLotNo,
								_ => _.RescanMode,
								_ => _.ErrorCd,
								_ => _.IsRescan,
								_ => _.IsActive
								);

				});

		}
	}
}
