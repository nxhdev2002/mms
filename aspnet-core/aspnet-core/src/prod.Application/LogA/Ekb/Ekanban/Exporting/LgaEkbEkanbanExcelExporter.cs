using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogA.Ekb.Dto;
using prod.LogA.Ekb.Ekanban.Dto;
using prod.LogA.Ekb.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.LogA.Ekb.Ekanban.Exporting
{
    public class LgaEkbEkanbanExcelExporter : NpoiExcelExporterBase, ILgaEkbEkanbanExcelExporter
    {
        public LgaEkbEkanbanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<LgaEkbEkanbanDto> progress)
        {
            return CreateExcelPackage(
                "LogAEkbEkanban.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Progress");
                    AddHeader(
                                sheet,
                                ("KanbanSeq"),
                                ("KanbanNoInDate"),
                                ("ProdLine"),
                                ("WorkingDate"),
                                ("Shift"),
                                ("ProgressId"),
                                ("ProcessId"),
                                ("PartListId"),
                                ("PartNo"),
                                ("PartNoNormalized"),
                                ("BackNo"),
                                ("PcAddress"),
                                ("SpsAddress"),
                                ("Sorting"),
                                ("RequestQty"),
                                ("ScanQty"),
                                ("InputQty"),
                                ("IsZeroKb"),
                                ("RequestDatetime"),
                                ("PikStartDatetime"),
                                ("PikFinishDatetime"),
                                ("DelStartDatetime"),
                                ("DelFinishDatetime"),
                                ("Status"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet, progress,
                                _ => _.KanbanSeq,
                                _ => _.KanbanNoInDate,
                                _ => _.ProdLine,
                                _ => _.WorkingDate,
                                _ => _.Shift,
                                _ => _.ProgressId,
                                _ => _.ProcessId,
                                _ => _.PartListId,
                                _ => _.PartNo,
                                _ => _.PartNoNormalized,
                                _ => _.BackNo,
                                _ => _.PcAddress,
                                _ => _.SpsAddress,
                                _ => _.Sorting,
                                _ => _.RequestQty,
                                _ => _.ScanQty,
                                _ => _.InputQty,
                                _ => _.IsZeroKb,
                                _ => _.RequestDatetime,
                                _ => _.PikStartDatetime,
                                _ => _.PikFinishDatetime,
                                _ => _.DelStartDatetime,
                                _ => _.DelFinishDatetime,
                                _ => _.Status,
                                _ => _.IsActive
                                );
                });

        }
    }
}
