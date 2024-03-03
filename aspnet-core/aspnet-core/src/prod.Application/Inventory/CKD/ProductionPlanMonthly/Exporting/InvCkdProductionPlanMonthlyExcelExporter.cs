using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.ProductionPlanMonthly.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.ProductionPlanMonthly.Exporting
{
    public class InvCkdProductionPlanMonthlyExcelExporter : NpoiExcelExporterBase, IInvCkdProductionPlanMonthlyExcelExporter
    {
        public InvCkdProductionPlanMonthlyExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<InvCkdProductionPlanMonthlyDto> invckdproductionplanmonthly)
        {
            return CreateExcelPackage(
              "InventoryCKDProdPlanMonthly.xlsx",
              excelPackage =>
              {
                  var sheet = excelPackage.CreateSheet("ProductionPlanMonthly");
                  AddHeader(
                              sheet,
                              ("Cfc"),
                              ("Grade"),
                              ("ProductionMonth"),
                              ("N - 3"),
                              ("N - 2"),
                              ("N - 1"),
                              ("N"),
                              ("N + 1"),
                              ("N + 2"),
                              ("N + 3"),
                              ("N + 4"),
                              ("N + 5"),
                              ("N + 6"),
                              ("N + 7"),
                              ("N + 8"),
                              ("N + 9"),
                              ("N + 10"),
                              ("N + 11"),
                              ("N + 12")
                            );
                  AddObjects(
                       sheet, invckdproductionplanmonthly,
                              _ => _.Cfc,
                              _ => _.Grade,
                              _ => _.ProductionMonth?.ToString("MM/yyyy"),
                              _ => _.N_3,
                              _ => _.N_2,
                              _ => _.N_1,
                              _ => _.N,
                              _ => _.N1,
                              _ => _.N2,
                              _ => _.N3,
                              _ => _.N4,
                              _ => _.N5,
                              _ => _.N6,
                              _ => _.N7,
                              _ => _.N8,
                              _ => _.N9,
                              _ => _.N10,
                              _ => _.N11,
                              _ => _.N12
                              );

              });
        }

        public FileDto ExportToFileErr(List<InvCkdProductionPlanMonthlyImportDto> listimporterr)
        {
            return CreateExcelPackage(
                "InvCkdProductionPlanMonthlyErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvCkdProdPlanMonthlyErr");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Cfc"),
                                ("Grade"),
                                ("N - 3"),
                                ("N - 2"),
                                ("N - 1"),
                                ("N"),
                                ("N + 1"),
                                ("N + 2"),
                                ("N + 3"),
                                ("N + 4"),
                                ("N + 5"),
                                ("N + 6"),
                                ("N + 7"),
                                ("N + 8"),
                                ("N + 9"),
                                ("N + 10"),
                                ("N + 11"),
                                ("N + 12"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, listimporterr,
                                _ => _.ROW_NO,
                                _ => _.Cfc,
                                _ => _.Grade,
                                _ => _.N_3,
                                _ => _.N_2,
                                _ => _.N_1,
                                _ => _.N,
                                _ => _.N1,
                                _ => _.N2,
                                _ => _.N3,
                                _ => _.N4,
                                _ => _.N5,
                                _ => _.N6,
                                _ => _.N7,
                                _ => _.N8,
                                _ => _.N9,
                                _ => _.N10,
                                _ => _.N11,
                                _ => _.N12,
                                _ => _.ErrorDescription
                        );
                });
        }
    }
}
