using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogA.Ekb.Dto;
using prod.Storage;
namespace prod.LogA.Ekb.Exporting
{
    public class LgaEkbProgressExcelExporter : NpoiExcelExporterBase, ILgaEkbProgressExcelExporter
    {
        public LgaEkbProgressExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<LgaEkbProgressDto> progress)
        {
            return CreateExcelPackage(
                "LogAEkbProgress.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Progress");
                    AddHeader(
                                sheet,
                                ("ProdLine"),
                                ("WorkingDate"),
                                ("Shift"),
                                ("NoInShift"),
                                ("NoInDate"),
                                ("ProcessId"),
                                ("ProcessCode"),
                                ("NewtaktDatetime"),
                                ("StartDatetime"),
                                ("FinishDatetime"),
                                ("Status"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet, progress,
                                _ => _.ProdLine,
                                _ => _.WorkingDate,
                                _ => _.Shift,
                                _ => _.NoInShift,
                                _ => _.NoInDate,
                                _ => _.ProcessId,
                                _ => _.ProcessCode,
                                _ => _.NewtaktDatetime,
                                _ => _.StartDatetime,
                                _ => _.FinishDatetime,
                                _ => _.Status,
                                _ => _.IsActive
                                );
                });

        }
    }
}
