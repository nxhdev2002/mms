using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Mwh.Exporting;
using prod.LogW.Mwh.Dto;
using prod.Storage;
using prod.LogW.Mwh.Dto;
namespace prod.LogW.Mwh.Exporting
{
	public class LgwMwhCaseDataExcelExporter : NpoiExcelExporterBase, ILgwMwhCaseDataExcelExporter
	{
		public LgwMwhCaseDataExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<LgwMwhCaseDataDto> casedata)
		{
			return CreateExcelPackage(
				"LogW_Mwh_CaseData.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("CaseData");
					AddHeader(
								sheet,
								("CaseNo"),
								("LotNo"),
								("Grade"),
								("Model"),
								("CaseQty"),
								("ContainerNo"),
								("Renban"),
								("SupplierNo"),
								("OrderType"),
								("CasePrefix"),
								("ProdLine"),
								("PxpCaseId"),
								("ContScheduleId"),
								("Status"),
								("DevanningDate"),
								("StartDevanningDate"),
								("FinishDevanningDate"),
								("ZoneCd"),
								("AreaCd"),
								("LocId"),
								("LocCd"),
								("LocDate"),
								("LocBy"),
								("Shop"),
								("IsBigpart"),
								("IsActive")
							   );
					AddObjects(
						 sheet, casedata,
								_ => _.CaseNo,
								_ => _.LotNo,
								_ => _.Grade,
								_ => _.Model,
								_ => _.CaseQty,
								_ => _.ContainerNo,
								_ => _.Renban,
								_ => _.SupplierNo,
								_ => _.OrderType,
								_ => _.CasePrefix,
								_ => _.ProdLine,
								_ => _.PxpCaseId,
								_ => _.ContScheduleId,
								_ => _.Status,
								_ => _.DevanningDate,
								_ => _.StartDevanningDate,
								_ => _.FinishDevanningDate,
								_ => _.ZoneCd,
								_ => _.AreaCd,
								_ => _.LocId,
								_ => _.LocCd,
								_ => _.LocDate,
								_ => _.LocBy,
								_ => _.Shop,
								_ => _.IsBigpart,
								_ => _.IsActive
								);

		
				});

		}

		public FileDto ExportWhBigCaseDataToFile(List<LgwMwhCaseDataDto> casedata)
		{
			return CreateExcelPackage(
				"LogW_Mwh_BigLocation_CaseData.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("CaseData");
					AddHeader(
								sheet,
								("CaseNo"),
								("SupplierNo"),
								("Loc"),
								("LocDetails")						
							   );
					AddObjects(
						 sheet, casedata,
								_ => _.CaseNo,
								_ => _.SupplierNo,
								_ => _.Loc,
								_ => _.LocDetails
								);
				});

		}

		public FileDto ExportRobbingDataToFile(List<LgwMwhCaseDataDto> casedata)
		{
			return CreateExcelPackage(
				"LogW_Mwh_Robbing_Localtion_CaseData.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("CaseData");
					AddHeader(
								sheet,
								("CaseNo"),
								("SupplierNo"),
								("Loc"),
								("LocDetails")
							   );
					AddObjects(
						 sheet, casedata,
								_ => _.CaseNo,
								_ => _.SupplierNo,
								_ => _.Loc,
								_ => _.LocDetails
								);

				
				});

		}
	}
}
