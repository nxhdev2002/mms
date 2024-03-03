using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;
namespace prod.Master.LogW.Exporting
{
	public class MstLgwPickingTabletProcessExcelExporter : NpoiExcelExporterBase, IMstLgwPickingTabletProcessExcelExporter
	{
		public MstLgwPickingTabletProcessExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgwPickingTabletProcessDto> pickingtabletsetup)
		{
			return CreateExcelPackage(
				"MasterLogWPickingTabletSetup.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PickingTabletSetup");
					AddHeader(
								sheet,
								("PickingTabletId"),
								("PickingPosition"),
								("Process"),
								("PickingCycle"),
								("LogicSequenceNo"),
								("LogicSequence"),
								("IsLotSupply"),
								("HasModel"),
								("IsActive")
							   );
					AddObjects(
						 sheet, pickingtabletsetup,
								_ => _.PickingTabletId,
								_ => _.PickingPosition,
								_ => _.Process,
								_ => _.PickingCycle,
								_ => _.LogicSequenceNo,
								_ => _.LogicSequence,
								_ => _.IsLotSupply,
								_ => _.HasModel,
								_ => _.IsActive
								);
				});

		}
	}
}
