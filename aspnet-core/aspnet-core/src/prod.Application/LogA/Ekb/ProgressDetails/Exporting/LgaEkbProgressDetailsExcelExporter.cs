using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogA.Ekb.Dto;
using prod.Storage;
namespace prod.LogA.Ekb.Exporting
{
    public class LgaEkbProgressDetailsExcelExporter : NpoiExcelExporterBase, ILgaEkbProgressDetailsExcelExporter
    {
        public LgaEkbProgressDetailsExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<LgaEkbProgressDetailsDto> progressdetails)
        {
            return CreateExcelPackage(
                "LogAEkbProgressDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ProgressDetails");
                    AddHeader(
                                sheet,
                                ("ProdLine"),
                                ("WorkingDate"),
                                ("Shift"),
                                ("NoInShift"),
                                ("NoInDate"),
                                ("ProgressId"),
                                ("ProcessId"),
                                ("PartListId"),
                                ("PartNo"),
                                ("PartNoNormalized"),
                                ("BackNo"),
                                ("PcAddress"),
                                ("SpsAddress"),
                                ("Sorting"),
                                ("UsageQty"),                     
                                ("SequenceNo"),
                                ("BodyNo"),
                                ("LotNo"),
                                ("NoInLot"),
                                ("Grade"),
                                ("Model"),
                                ("BodyColor"),
                                ("EkbQty"),
                                ("RemainQty"),
                                ("IsZeroKb"),
                                ("NewtaktDatetime"),
                                ("PikStartDatetime"),
                                ("PikFinishDatetime"),
                                ("DelStartDatetime"),
                                ("DelFinishDatetime"),
                                ("KanbanSeq"),
                                ("Status"),
                                ("IsActive")
                                );
                    AddObjects(
                         sheet, progressdetails,
                                _ => _.ProdLine,
                                _ => _.WorkingDate,
                                _ => _.Shift,
                                _ => _.NoInShift,
                                _ => _.NoInDate,
                                _ => _.ProgressId,
                                _ => _.ProcessId,
                                _ => _.PartListId,
                                _ => _.PartNo,
                                _ => _.PartNoNormalized,
                                _ => _.BackNo,
                                _ => _.PcAddress,
                                _ => _.SpsAddress,
                                _ => _.Sorting,
                                _ => _.UsageQty,
                                _ => _.SequenceNo,
                                _ => _.BodyNo,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.Grade,
                                _ => _.Model,
                                _ => _.BodyColor,
                                _ => _.EkbQty,
                                _ => _.RemainQty,
                                _ => _.IsZeroKb,
                                _ => _.NewtaktDatetime,
                                _ => _.PikStartDatetime,
                                _ => _.PikFinishDatetime,
                                _ => _.DelStartDatetime,
                                _ => _.DelFinishDatetime,
                                _ => _.KanbanSeq,
                                _ => _.Status,
                                _ => _.Status,
                                _ => _.IsActive

                                );
                });

        }
    }
}
