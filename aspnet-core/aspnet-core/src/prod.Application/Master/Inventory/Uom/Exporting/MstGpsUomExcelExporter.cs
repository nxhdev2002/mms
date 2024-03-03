using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.Dto;
using prod.Storage;
namespace prod.Master.Inventory.Exporting
{
    public class MstGpsUomExcelExporter : NpoiExcelExporterBase, IMstGpsUomExcelExporter
    {
        public MstGpsUomExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstGpsUomDto> uom)
        {
            return CreateExcelPackage(
                "MasterGpsUom.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Uom");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name")

                               );
                    AddObjects(
                         sheet, uom,
                                _ => _.Code,
                                _ => _.Name

                                );
                });

        }
    }
}
