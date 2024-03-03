using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Plm.Dto;
using prod.Storage;


namespace prod.Master.Plm.Exporting
{
    public class MasterPlmMatrixLotCodeExcelExporter : NpoiExcelExporterBase, IMasterPlmMatrixLotCodeExcelExporter
    {
        public MasterPlmMatrixLotCodeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MasterPlmMatrixLotCodeDto> matrixlotcode)
        {
            return CreateExcelPackage(
                "MasterPlmMatrixLotCode.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("MatrixLotCode");
                    AddHeader(
                                sheet,
                                ("ScreenId"),
                                ("LotcodeGradeId"),
                                ("PartId")

                               );
                    AddObjects(
                         sheet,matrixlotcode,
                                _ => _.ScreenId,
                                _ => _.LotcodeGradeId,
                                _ => _.PartId

                                );

                 
                });

        }
    }
}
