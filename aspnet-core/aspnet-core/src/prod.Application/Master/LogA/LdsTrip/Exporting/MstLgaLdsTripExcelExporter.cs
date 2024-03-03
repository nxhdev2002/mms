using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Storage;
using prod.Dto;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA.Exporting
{
	public class MstLgaLdsTripExcelExporter : NpoiExcelExporterBase, IMstLgaLdsTripExcelExporter
	{
		public MstLgaLdsTripExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstLgaLdsTripDto> trip)
		{
			return CreateExcelPackage(
				"MasterLogATrip.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("Trip");
					AddHeader(
								sheet,
								("ProdLine"),
								("DeliveryNo"),
								("DeliveryName"),
                                ("Model"),
                                ("TripNumber"),
								("DollyName"),
								("TaktTime"),
								("IsActive")
							   );
					AddObjects(
						 sheet, trip,
								_ => _.ProdLine,
								_ => _.DeliveryNo,
								_ => _.DeliveryName,
                                _ => _.Model,
                                _ => _.TripNumber,
								_ => _.DollyName,
								_ => _.TaktTime,
								_ => _.IsActive
								);
				});

		}
	}
}
