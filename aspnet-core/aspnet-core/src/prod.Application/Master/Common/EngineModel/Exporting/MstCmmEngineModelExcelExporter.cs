using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Cmm.Dto;
using prod.Storage;
using prod.Master.Cmm.Exporting;

namespace vovina.Master.Cmm.Exporting
{
    public class MstCmmEngineModelExcelExporter : NpoiExcelExporterBase, IMstCmmEngineModelExcelExporter
    {
        public MstCmmEngineModelExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmEngineModelDto> enginemodel)
        {
            return CreateExcelPackage(
                "MasterCmmEngineModel.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("EngineModel");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name")

                               );
                    AddObjects(
                         sheet, enginemodel,
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