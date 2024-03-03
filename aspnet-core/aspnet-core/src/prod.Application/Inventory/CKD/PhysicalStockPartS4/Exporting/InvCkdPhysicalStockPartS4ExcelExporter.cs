using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using prod.Inv.CKD.Dto;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdPhysicalStockPartS4ExcelExporter : NpoiExcelExporterBase, IInvCkdPhysicalStockPartS4ExcelExporter
    {
        public InvCkdPhysicalStockPartS4ExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdPhysicalStockPartS4Dto> modulecase)
        {
            return CreateExcelPackage(
                "PhysicalStockPartS4Compare.xlsx",
                excelPackage =>
                {
                var sheet = excelPackage.CreateSheet("PhysicalStockPartS4Compare");
                AddHeader(
                        sheet,
                        ("MaterialCodeS4"),
                        ("QtyS4"),
                        ("MaterialCode"),
                        ("QtyS4"),
                        ("Diff"),
                        ("From Date"),
                        ("To Date"));
                    AddObjects(
                    sheet, modulecase,
                        _=> _.MaterialCodeS4,
                        _ => _.QtyS4,
                        _ => _.MaterialCode,
                        _ => _.Qty,
                        _ => _.Diff,  
                        _ => _.FromDate,
                        _ => _.ToDate
                        );
                });

        }

        public FileDto ExportListErrToFile(List<InvCkdPhysicalStockPartS4Dto> stockPartS4Err)
        {
            return CreateExcelPackage(
                "PhysicalStockPartS4Err.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PhysicalStockPartS4Err");
                    AddHeader(
                            sheet,
                            ("MaterialCode"),
                            ("PeriodId"),
                            ("Qty"),
                            ("ErrorDescription")
                            );
                    AddObjects(
                    sheet, stockPartS4Err,
                        _ => _.MaterialCode,
                        _ => _.PeriodId,
                        _ => _.Qty,
                        _ => _.ErrorDescription
                        );
                });
        }
    }
}