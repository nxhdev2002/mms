using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;
namespace prod.Master.LogW.Exporting
{
	public class MstLgwScreenConfigExcelExporter : NpoiExcelExporterBase, IMstLgwScreenConfigExcelExporter
	{
		public MstLgwScreenConfigExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwScreenConfigDto> screenconfig)
		{
			return CreateExcelPackage(
				"MasterLogWScreenConfig.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("ScreenConfig");
					AddHeader(
								sheet,
								("UnpackDoneColor"),
								("NeedToUnpackColor"),
								("NeedToUnpackFlash"),
								("ConfirmFlagColor"),
								("ConfirmFlagSound"),
								("ConfirmFlagPlaytime"),
								("ConfirmFlagFlash"),
								("DelayUnpackColor"),
								("DelayUnpackSound"),
								("DelayUnpackPlaytime"),
								("DelayUnpackFlash"),
								("CallLeaderColor"),
								("CallLeaderSound"),
								("CallLeaderPlaytime"),
								("CallLeaderFlash"),
								("TotalColumnOldShift"),
								("TotalColumnSeqA1"),
								("TotalColumnSeqA2"),
								("BeforeTacktimeColor"),
								("BeforeTacktimeSound"),
								("BeforeTacktimePlaytime"),
								("BeforeTacktimeFlash"),
								("TackCaseA1"),
								("TackCaseA2"),
								("IsActive")
							   );
					AddObjects(
						 sheet, screenconfig,
								_ => _.UnpackDoneColor,
								_ => _.NeedToUnpackColor,
								_ => _.NeedToUnpackFlash,
								_ => _.ConfirmFlagColor,
								_ => _.ConfirmFlagSound,
								_ => _.ConfirmFlagPlaytime,
								_ => _.ConfirmFlagFlash,
								_ => _.DelayUnpackColor,
								_ => _.DelayUnpackSound,
								_ => _.DelayUnpackPlaytime,
								_ => _.DelayUnpackFlash,
								_ => _.CallLeaderColor,
								_ => _.CallLeaderSound,
								_ => _.CallLeaderPlaytime,
								_ => _.CallLeaderFlash,
								_ => _.TotalColumnOldShift,
								_ => _.TotalColumnSeqA1,
								_ => _.TotalColumnSeqA2,
								_ => _.BeforeTacktimeColor,
								_ => _.BeforeTacktimeSound,
								_ => _.BeforeTacktimePlaytime,
								_ => _.BeforeTacktimeFlash,
								_ => _.TackCaseA1,
								_ => _.TackCaseA2,
								_ => _.IsActive
								);
				});

		}
	}
}
