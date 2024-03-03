using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Exporting;
using prod.Storage;
using System.Collections.Generic;
namespace prod.Master.LogA.Exporting
{
    public class MstLgaEkbPartListExcelExporter : NpoiExcelExporterBase, IMstLgaEkbPartListExcelExporter
    {
        public MstLgaEkbPartListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            TempFileCacheManager = tempFileCacheManager;
        }

        public ITempFileCacheManager TempFileCacheManager { get; }

        public FileDto ExportToFile(List<MstLgaEkbPartListDto> ekbpartlist)
        {
            return CreateExcelPackage(
                "MasterLogAEkbPartList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("EkbPartList");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartNoNormanlized"),
                                ("PartName"),
                                ("BackNo"),
                                ("ProdLine"),
                                ("SupplierNo"),
                                ("Model"),
                                ("ProcessId"),
                                ("ProcessCode"),
                                ("UsageQty"),
                                ("BoxQty"),
                                ("PcAddress"),
                                ("PcSorting"),
                                ("SpsAddress"),
                                ("SpsSorting"),
                                ("Remark"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, ekbpartlist,
                                _ => _.PartNo,
                                _ => _.PartNoNormanlized,
                                _ => _.PartName,
                                _ => _.BackNo,
                                _ => _.ProdLine,
                                _ => _.SupplierNo,
                                _ => _.Model,
                                _ => _.ProcessId,
                                _ => _.ProcessCode,
                                _ => _.UsageQty,
                                _ => _.BoxQty,
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
