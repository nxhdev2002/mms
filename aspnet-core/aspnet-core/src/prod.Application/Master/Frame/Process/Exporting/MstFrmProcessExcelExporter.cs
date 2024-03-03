using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Frame.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Master.Frame.Exporting
{
	public class MstFrmProcessExcelExporter : NpoiExcelExporterBase, IMstFrmProcessExcelExporter
	{
		public MstFrmProcessExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstFrmProcessDto> process)
		{
			return CreateExcelPackage(
				"MasterFrameProcess.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("Process");
					AddHeader(
								sheet,
								("ProcessSeq"),
								("ProcessCode"),
								("ProcessName"),
								("ProcessDesc"),
								("ProcessGroup"),
								("GroupName"),
								("ProcessSubgroup"),
								("SubgroupName"),
								("IsActive")
							   );
					AddObjects(
						 sheet, process,
								_ => _.ProcessSeq,
								_ => _.ProcessCode,
								_ => _.ProcessName,
								_ => _.ProcessDesc,
								_ => _.ProcessGroup,
								_ => _.GroupName,
								_ => _.ProcessSubgroup,
								_ => _.SubgroupName,
								_ => _.IsActive
								);
				});

		}
	}
}
