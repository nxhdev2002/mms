using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.Dto;
using prod.Storage;
using prod.Master.Inventory.Dto;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using prod.Storage;

namespace prod.Master.Inventory.Exporting
{
    public class MstInvCustomsPortExcelExporter : NpoiExcelExporterBase, IMstInvCustomsPortExcelExporter
    {
        public MstInvCustomsPortExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvCustomsPortDto> customsport)
        {
            return CreateExcelPackage(
                "MasterInventoryCustomsPort.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CustomsPort");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name"),
                                ("AccountNumber"),
                                ("BankName"),
                                ("VendorNumber"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, customsport,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.AccountNumber,
                                _ => _.BankName,
                                _ => _.VendorNumber,
                                _ => _.IsActive

                                );
                });

        }
    }
}
