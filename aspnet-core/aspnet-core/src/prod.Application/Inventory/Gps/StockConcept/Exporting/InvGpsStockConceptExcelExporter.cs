using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Inventory.GPS.Dto;
using prod.Storage;
using prod.Inventory.GPS.Dto;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Storage;

namespace prod.Inventory.GPS.Exporting
{
    public class InvGpsStockConceptExcelExporter : NpoiExcelExporterBase, IInvGpsStockConceptExcelExporter
    {
        public InvGpsStockConceptExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvGpsStockConceptDto> invgpsstockconcept)
        {
            return CreateExcelPackage(
                "InventoryGPSInvGpsStockConcept.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvGpsStockConcept");
                    AddHeader(
                                sheet,
                                ("SupplierCode"),
                                ("MonthStk"),
                                ("MinStk1"),
                                ("MinStk2"),
                                ("MinStk3"),
                                ("MinStk4"),
                                ("MinStk5"),
                                ("MinStk6"),
                                ("MinStk7"),
                                ("MinStk8"),
                                ("MinStk9"),
                                ("MinStk10"),
                                ("MinStk11"),
                                ("MinStk12"),
                                ("MinStk13"),
                                ("MinStk14"),
                                ("MinStk15"),
                                ("MaxStk1"),
                                ("MaxStk2"),
                                ("MaxStk3"),
                                ("MaxStk4"),
                                ("MaxStk5"),
                                ("MinStkConcept"),
                                ("MaxStkConcept"),
                                ("StkConcept"),
                                ("StkConceptFrq"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, invgpsstockconcept,
                                _ => _.SupplierCode,
                                _ => _.MonthStk,
                                _ => _.MinStk1,
                                _ => _.MinStk2,
                                _ => _.MinStk3,
                                _ => _.MinStk4,
                                _ => _.MinStk5,
                                _ => _.MinStk6,
                                _ => _.MinStk7,
                                _ => _.MinStk8,
                                _ => _.MinStk9,
                                _ => _.MinStk10,
                                _ => _.MinStk11,
                                _ => _.MinStk12,
                                _ => _.MinStk13,
                                _ => _.MinStk14,
                                _ => _.MinStk15,
                                _ => _.MaxStk1,
                                _ => _.MaxStk2,
                                _ => _.MaxStk3,
                                _ => _.MaxStk4,
                                _ => _.MaxStk5,
                                _ => _.MinStkConcept,
                                _ => _.MaxStkConcept,
                                _ => _.StkConcept,
                                _ => _.StkConceptFrq,
                                _ => _.IsActive

                                );
                });

        }
    }
}