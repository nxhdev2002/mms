using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Master.Inventory.Exporting
{
    public class MstInvShippingCompanyExcelExporter : NpoiExcelExporterBase, IMstInvShippingCompanyExcelExporter
    {
        public MstInvShippingCompanyExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvShippingCompanyDto> shippingcompany)
        {
            return CreateExcelPackage(
                "MasterInventoryShippingCompany.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ShippingCompany");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name"),
                                ("Description"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, shippingcompany,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.Description,
                                _ => _.IsActive

                                );


                });

        }
    }
}
