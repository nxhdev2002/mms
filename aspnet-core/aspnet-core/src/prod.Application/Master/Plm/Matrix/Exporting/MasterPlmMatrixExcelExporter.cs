using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Plm.Exporting;
using prod.Master.Plm.Dto;
using prod.Storage;
using prod.Master.Plm.Dto;
namespace prod.Master.Plm.Exporting
{
    public class MasterPlmMatrixExcelExporter : NpoiExcelExporterBase, IMasterPlmMatrixExcelExporter
    {
        public MasterPlmMatrixExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MasterPlmMatrixDto> matrix)
        {
            return CreateExcelPackage(
                "MasterPlmMatrix.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Matrix");
                    AddHeader(
                                sheet,
                                ("ScreenId"),
                                ("PartId"),
                                ("Ordering"),
                                ("SideId")

                               );
                    AddObjects(
                         sheet, matrix,
                                _ => _.ScreenId,
                                _ => _.PartId,
                                _ => _.Ordering,
                                _ => _.SideId

                                );
                });

        }
    }
}
