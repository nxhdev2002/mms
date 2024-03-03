using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Welding.Andon.Exporting;
using prod.Welding.Andon.Dto;
using prod.Storage;
using prod.Welding.Andon.Dto;
namespace prod.Welding.Andon.Exporting
{
	public class WldAdoWeldingProgressExcelExporter : NpoiExcelExporterBase, IWldAdoWeldingProgressExcelExporter
	{
		public WldAdoWeldingProgressExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<WldAdoWeldingProgressDto> weldingprogress)
		{
			return CreateExcelPackage(
				"WeldingAndonWeldingProgress.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("WeldingProgress");
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
						 sheet, weldingprogress,
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
