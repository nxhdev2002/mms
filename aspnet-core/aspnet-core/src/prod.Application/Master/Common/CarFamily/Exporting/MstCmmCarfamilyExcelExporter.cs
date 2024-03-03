using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Dto;
using prod.Storage;
using prod.Master.Common.Dto;
namespace prod.Master.Common.Exporting
{
    public class MstCmmCarfamilyExcelExporter : NpoiExcelExporterBase, IMstCmmCarfamilyExcelExporter
    {
        public MstCmmCarfamilyExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmCarfamilyDto> carfamily)
        {
            return CreateExcelPackage(
                "MasterCommonCarfamily.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Carfamily");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name")

                               );
                    AddObjects(
                         sheet,  carfamily,
                                _ => _.Code,
                                _ => _.Name

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
