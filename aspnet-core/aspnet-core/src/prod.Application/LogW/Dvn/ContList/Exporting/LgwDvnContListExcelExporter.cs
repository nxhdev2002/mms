using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Dvn.Exporting;
using prod.LogW.Dvn.Dto;
using prod.Storage;
using prod.LogW.Dvn.Dto;
namespace prod.LogW.Dvn.Exporting
{
	public class LgwDvnContListExcelExporter : NpoiExcelExporterBase, ILgwDvnContListExcelExporter
	{
		public LgwDvnContListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgwDvnContListDto> contlist)
		{
			return CreateExcelPackage(
				"LogWDvnContList.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("ContList");
					AddHeader(
								sheet,
								("ContainerNo"),
								("Renban"),
								("SupplierNo"),
								("LotNo"),
								("WorkingDate"),
								("ShiftNo"),
								("DevanningDock"),
								("PlanDevanningDate"),
								("ActDevanningDate"),
								("ActDevanningDateFinished"),
								("DevanningType"),
								("Status"),
								("DevLeadtime"),
								("PlanDevanningLineOff"),
								("SortingStatus"),
								("IsActive")
							   );
					AddObjects(
						 sheet, contlist,
								_ => _.ContainerNo,
								_ => _.Renban,
								_ => _.SupplierNo,
								_ => _.LotNo,
								_ => _.WorkingDate,
								_ => _.ShiftNo,
								_ => _.DevanningDock,
								_ => _.PlanDevanningDate,
								_ => _.ActDevanningDate,
								_ => _.ActDevanningDateFinished,
								_ => _.DevanningType,
								_ => _.Status,
								_ => _.DevLeadtime,
								_ => _.PlanDevanningLineOff,
								_ => _.SortingStatus,
								_ => _.IsActive
								);
				});

		}
	}
}
