using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Storage;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Storage;
using prod.Master.Inventory.Forwarder.Dto;
using prod.Master.Inventory.Forwarder;

namespace prod.Master.Inventory.Exporting
{
    public class MstInvForwarderExcelExporter : NpoiExcelExporterBase, IMstInvForwarderExcelExporter
    {
        public MstInvForwarderExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvForwarderDto> forwarder)
        {
            return CreateExcelPackage(
                "MasterInventoryForwarder.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Forwarder");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name"),
                                ("SupplierNo"),
                                ("ShippingNo"),
                                ("EfDatefrom"),
                                ("EfDateto"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, forwarder,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.SupplierNo,
                                _ => _.ShippingNo,
                                _ => _.EfDatefrom,
                                _ => _.EfDateto,
                                _ => _.IsActive

                                );
                });

        }
    }
}
