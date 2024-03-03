using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using prod.Storage;
using System.Collections.Generic;

namespace vovina.Master.Inventory.Exporting
{
    public class MstInvCkdRentalWarehouseExcelExporter : NpoiExcelExporterBase, IMstInvCkdRentalWarehouseExcelExporter
    {
        public MstInvCkdRentalWarehouseExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvCkdRentalWarehouseDto> cpsinventorygroup)
        {
            return CreateExcelPackage(
                "MstInvCkdRentalWarehouse.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("MstInvCkdRentalWarehouse");
                    AddHeader(
                                sheet,
                                    ("Code"),
                                    ("Name"),
                                    ("From Date"),
                                    ("To Date"),
									("IsActive")
                                   );
                    AddObjects(
                         sheet, cpsinventorygroup,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.FromDate,
                                _ => _.ToDate,
								_ => _.IsActive

                                );
                });

        }
    }
}
