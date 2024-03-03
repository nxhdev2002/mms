using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.Dto;
using prod.Storage;
using prod.Master.Inventory.Dto;
namespace prod.Master.Inventory.Exporting
{
    public class MstInvCpsSuppliersExcelExporter : NpoiExcelExporterBase, IMstInvCpsSuppliersExcelExporter
    {
        public MstInvCpsSuppliersExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvCpsSuppliersDto> cpssuppliers)
        {
            return CreateExcelPackage(
                "MasterInventoryCpsSuppliers.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CpsSuppliers");
                    AddHeader(
                                sheet,
                                ("SupplierName"),
                                ("SupplierNumber"),
                                ("VatregistrationNum"),
                                ("VatregistrationInvoice"),
                                ("TaxPayerId"),
                                ("RegistryId"),
                                ("StartDateActive"),
                                ("EndDateActive"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, cpssuppliers,
                                _ => _.SupplierName,
                                _ => _.SupplierNumber,
                                _ => _.VatregistrationNum,
                                _ => _.VatregistrationInvoice,
                                _ => _.TaxPayerId,
                                _ => _.RegistryId,
                                _ => _.StartDateActive,
                                _ => _.EndDateActive,
                                _ => _.IsActive

                                );
                });

        }
    }
}
