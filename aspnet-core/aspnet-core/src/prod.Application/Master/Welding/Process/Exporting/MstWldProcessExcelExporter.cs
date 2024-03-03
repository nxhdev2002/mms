using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Welding.Dto;
using prod.Storage;
using System.Collections.Generic;
namespace prod.Master.Welding.Exporting
{
	public class MstWldProcessExcelExporter : NpoiExcelExporterBase, IMstWldProcessExcelExporter
	{
		public MstWldProcessExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstWldProcessDto> process)
		{
			return CreateExcelPackage(
				"MasterWeldingProcess.xlsx",
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
