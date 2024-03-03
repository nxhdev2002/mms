using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Mwh.Exporting;
using prod.LogW.Mwh.Dto;
using prod.Storage;
using prod.LogW.Mwh.Dto;
namespace prod.LogW.Mwh.Exporting
{
	public class LgwMwhPartDataExcelExporter : NpoiExcelExporterBase, ILgwMwhPartDataExcelExporter
	{
		public LgwMwhPartDataExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgwMwhPartDataDto> partdata)
		{
			return CreateExcelPackage(
				"LogWMwhPartData.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("PartData");
					AddHeader(
								sheet,
								("PxppartId"),
								("PartNo"),
								("LotNo"),
								("Fixlot"),
								("CaseNo"),
								("ModuleNo"),
								("ContainerNo"),
								("SupplierNo"),
								("UsageQty"),
								("PartName"),
								("CarfamilyCode"),
								("InvoiceParentId"),
								("PxpcaseId"),
								("OrderType"),
								("IsActive")
							   );
					AddObjects(
						 sheet, partdata,
								_ => _.PxppartId,
								_ => _.PartNo,
								_ => _.LotNo,
								_ => _.Fixlot,
								_ => _.CaseNo,
								_ => _.ModuleNo,
								_ => _.ContainerNo,
								_ => _.SupplierNo,
								_ => _.UsageQty,
								_ => _.PartName,
								_ => _.CarfamilyCode,
								_ => _.InvoiceParentId,
								_ => _.PxpcaseId,
								_ => _.OrderType,
								_ => _.IsActive
								);
				});

		}
	}
}

