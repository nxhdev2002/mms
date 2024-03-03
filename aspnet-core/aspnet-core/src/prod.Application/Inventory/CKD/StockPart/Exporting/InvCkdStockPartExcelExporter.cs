using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using prod.Inventory.CKD.Dto;
namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdStockPartExcelExporter : NpoiExcelExporterBase, IInvCkdStockPartExcelExporter
	{
		public InvCkdStockPartExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<InvCkdStockPartDto> stockpart)
		{
			return CreateExcelPackage(
				"InventoryCKDStockPart.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("StockPart");
					AddHeader(
								sheet,
								("PartNo"),
								("PartNoNormalized"),
								("PartName"),
								("PartNoNormalizedS4"),
								("ColorSfx"),
								("PartListId"),
								("MaterialId"),
								("Qty"),
								("WorkingDate"),
								("PeriodId"),
								("LastCalDatetime"),
								("IsActive")
							   );
					AddObjects(
						 sheet, stockpart,
								_ => _.PartNo,
								_ => _.PartNoNormalized,
								_ => _.PartName,
								_ => _.PartNoNormalizedS4,
								_ => _.ColorSfx,
								_ => _.PartListId,
								_ => _.MaterialId,
								_ => _.Qty,
								_ => _.WorkingDate,
								_ => _.PeriodId,
								_ => _.LastCalDatetime,
								_ => _.IsActive
								);

					
				});

		}

        public FileDto ExportToFileCheckStock(List<InvCkdStockReceivingDto> checkstockpart)
        {
            return CreateExcelPackage(
                "InvCKDStockPartCheckStock.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CheckStockPart");
                    AddHeader(
                                sheet,
                                ("Part No"),
                                ("Part Name"),
                                ("Error Description")
                               );
                    AddObjects(
                         sheet, checkstockpart,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.ErrDes
                                );
                });
        }

        public FileDto ExportByMaterialToFile(List<InvCkdStockPartByMaterialDto> invckdstockpartbymaterial)
        {
            return CreateExcelPackage(
                "InvCkdStockPartByMaterial.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StockPartByMasterial");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("Description"),
                                ("ValuationType"),
                                ("Qty"),
                                ("WorkingDate"),
                                ("LastCalDatetime"),
                                ("MaterialId")
                               );
                    AddObjects(
                         sheet, invckdstockpartbymaterial,
                                _ => _.PartNo,
                                _ => _.Description,
                                _ => _.ValuationType,
                                _ => _.Qty,
                                _ => _.WorkingDate,
                                _ => _.LastCalDatetime,
                                _ => _.MaterialId
                                );


                });

        }
    }
}
