using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CPS.Dto;
using prod.Inventory.CPS.Exporting;
using prod.Storage;

namespace prod.Inventory.CPS.Exporting
{
    public class InvCpsPoHeadersExcelExporter : NpoiExcelExporterBase, IInvCpsPoHeadersExcelExporter
    {
        public InvCpsPoHeadersExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<GridPoHeadersDto> poheaders)
        {
            return CreateExcelPackage(
                "InventoryCPSPoHeaders.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PoHeaders");
                    AddHeader(
                                sheet,
                                ("Po Number"),
                                ("Creat Date"),
                                ("Comments"),
                                ("Type"),
                                ("Inventory Group"),
                                ("Vendor Name"),
                                ("Total Price")
                               );
                    AddObjects(
                         sheet, poheaders,
                                _ => _.PoNumber,
                                _ => _.CreationTime,
                                _ => _.Comments,
                                _ => _.TypeLookupCode,
                                _ => _.Productgroupname,
                                _ => _.SupplierName,
                                _ => _.TotalPrice
        
                                );

                });

        }
     
        public FileDto ExportToFilePoline(List<InvCpsPoLinesDto> polines)
        {
            return CreateExcelPackage(
                "InventoryCPSPoLines.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PoLines");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("Color"),
                                ("PartName"),
                                ("UnitMeasLookupCode"),
                                ("ListPricePerUnit"),
                                ("UnitPrice"),
                                ("Quantity")
                               );
                    AddObjects(
                         sheet, polines,
                                _ => _.PartNo,
                                _ => _.Color,
                                _ => _.ItemDescription,
                                _ => _.UnitMeasLookupCode,
                                _ => _.ListPricePerUnit,
                                _ => _.UnitPrice,
                                _ => _.Quantity
                                );

                });

        }
    }
}
