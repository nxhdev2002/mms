using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CPS.Dto;
using prod.Inventory.CPS.Exporting;
using prod.Storage;
using System.Collections.Generic;
namespace prod.Inventory.CPS.Exporting
{
    public class InvCpsInventoryItemPriceExcelExporter : NpoiExcelExporterBase, IInvCpsInventoryItemPriceExcelExporter
    {
        public InvCpsInventoryItemPriceExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCpsInventoryItemPriceDto> invoicelines)
        {
            return CreateExcelPackage(
                "InvCpsInventoryItemPrice.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InventoryItemPrice");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("Color"),
                                ("PartName"),
                                ("PartNameSupplier"),
                                ("SupplierName"),
                                ("CurrencyCode"),
                                ("UnitPrice"),
                                ("TaxPrice"),
                                ("EffectiveFrom"),
                                ("EffectiveTo"),
                                ("PartNoCPS"),
                                ("ProductGroupName"),
                                ("UnitMeasLookupCode"),
                                ("ApproveDate")

                               );
                    AddObjects(
                         sheet, invoicelines,
                                _ => _.PartNo,
                                _ => _.Color,
                                _ => _.PartName,
                                _ => _.PartNameSupplier,
                                _ => _.SupplierName,
                                _ => _.CurrencyCode,
                                _ => _.UnitPrice,
                                _ => _.TaxPrice,
                                _ => _.EffectiveFrom,
                                _ => _.EffectiveTo,
                                _ => _.PartNoCPS,
                                _ => _.ProductGroupName,
                                _ => _.UnitMeasLookupCode,
                                _ => _.ApproveDate

                                );


                });

        }
    }
}