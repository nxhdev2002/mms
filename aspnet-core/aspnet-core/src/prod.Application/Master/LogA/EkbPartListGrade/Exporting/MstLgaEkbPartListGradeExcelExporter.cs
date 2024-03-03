using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogA.Dto;
using prod.Storage;
using System.Collections.Generic;
namespace prod.Master.LogA.Exporting
{
    public class MstLgaEkbPartListGradeExcelExporter : NpoiExcelExporterBase, IMstLgaEkbPartListGradeExcelExporter
    {
        public MstLgaEkbPartListGradeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstLgaEkbPartListGradeDto> ekbpartlistgrade)
        {
            return CreateExcelPackage(
                "MasterLogAEkbPartListGrade.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("EkbPartListGrade");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartNoNormanlized"),
                                ("PartName"),
                                ("BackNo"),
                                ("PartListId"),
                                ("ProdLine"),
                                ("SupplierNo"),
                                ("Model"),
                                ("ProcessId"),
                                ("ProcessCode"),
                                ("Grade"),
                                ("UsageQty"),
                                ("BoxQty"),
                                ("Module"),
                                ("PcAddress"),
                                ("PcSorting"),
                                ("SpsAddress"),
                                ("SpsSorting"),
                                ("Remark"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, ekbpartlistgrade,
                                _ => _.PartNo,
                                _ => _.PartNoNormanlized,
                                _ => _.PartName,
                                _ => _.BackNo,
                                _ => _.PartListId,
                                _ => _.ProdLine,
                                _ => _.SupplierNo,
                                _ => _.Model,
                                _ => _.ProcessId,
                                _ => _.ProcessCode,
                                _ => _.Grade,
                                _ => _.UsageQty,
                                _ => _.BoxQty,
                                _ => _.Module,
                                _ => _.PcAddress,
                                _ => _.PcSorting,
                                _ => _.SpsAddress,
                                _ => _.SpsSorting,
                                _ => _.Remark,
                                _ => _.IsActive

                                );
                });

        }
    }
}
