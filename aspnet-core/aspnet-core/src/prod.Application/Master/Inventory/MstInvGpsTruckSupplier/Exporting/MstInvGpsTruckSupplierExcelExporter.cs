using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inv.Dto;
using prod.Master.Inv.Exporting;
using prod.Storage;

namespace prod.Master.Inv.Exporting
{
    public class MstInvGpsTruckSupplierExcelExporter : NpoiExcelExporterBase, IMstInvGpsTruckSupplierExcelExporter
    {
        public MstInvGpsTruckSupplierExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvGpsTruckSupplierDto> gpstrucksupplier)
        {
            return CreateExcelPackage(
                "MasterInvGpsTruckSupplier.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsTruckSupplier");
                    AddHeader(
                                sheet,
                                ("SupplierId"),
                                ("TruckName"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, gpstrucksupplier,
                                _ => _.SupplierId,
                                _ => _.TruckName,
                                _ => _.IsActive

                                );
                });

        }
    }
}
