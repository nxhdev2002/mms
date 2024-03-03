using NPOI.SS.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Storage;
using System.Collections.Generic;
using prod.Master.Inventory.Dto;
namespace prod.Master.Inventory.Exporting
{
    public class MstInvInvoiceStatusExcelExporter : NpoiExcelExporterBase, IMstInvInvoiceStatusExcelExporter
    {
        public MstInvInvoiceStatusExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvInvoiceStatusDto> invoicestatus)
        {
            return CreateExcelPackage(
                "MasterInventoryInvoiceStatus.xlsx",
                excelPackage =>
                {
                var sheet = excelPackage.CreateSheet("InvoiceStatus");
                AddHeader(
                            sheet,
                            ("Code"),
								("Description")
							   );
            AddObjects(
                 sheet, invoicestatus,
            _ => _.Code,
                        _ => _.Description

                        );

        
        });

        }
}
}