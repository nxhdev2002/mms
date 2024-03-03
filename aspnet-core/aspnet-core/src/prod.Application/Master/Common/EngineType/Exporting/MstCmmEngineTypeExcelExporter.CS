using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Master.Cmm.Exporting;
using prod.Storage;
using prod.Master.Cmm.Dto;
using prod.Dto;

namespace vovina.Master.Cmm.Exporting
{
    public class MstCmmEngineTypeExcelExporter : NpoiExcelExporterBase, IMstCmmEngineTypeExcelExporter
    {
        public MstCmmEngineTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmEngineTypeDto> enginetype)
        {
            return CreateExcelPackage(
                "MasterCmmEngineType.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("EngineType");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name")

                               );
                    AddObjects(
                         sheet, enginetype,
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
