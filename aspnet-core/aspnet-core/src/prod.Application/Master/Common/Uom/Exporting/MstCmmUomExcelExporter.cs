using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Dto;
using prod.Storage;
using prod.Master.Common.Dto;
namespace prod.Master.Common.Exporting
{
    public class MstCmmUomExcelExporter : NpoiExcelExporterBase, IMstCmmUomExcelExporter
    {
        public MstCmmUomExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmUomDto> uom)
        {
            return CreateExcelPackage(
                "MasterCommonUom.xlsx",
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
