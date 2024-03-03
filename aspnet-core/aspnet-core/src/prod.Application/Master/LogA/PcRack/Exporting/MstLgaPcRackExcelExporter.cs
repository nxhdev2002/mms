using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogA.Dto;
using prod.Storage;

namespace prod.Master.LogA.Exporting
{
    public class MstLgaPcRackExcelExporter : NpoiExcelExporterBase, IMstLgaPcRackExcelExporter
    {
        public MstLgaPcRackExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstLgaPcRackDto> pcrack)
        {
            return CreateExcelPackage(
                "MasterLogAPcRack.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PcRack");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Address"),
                                ("Ordering"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet, pcrack,
                                _ => _.Code,
                                _ => _.Address,
                                _ => _.Ordering,
                                _ => _.IsActive

                                );
                });

        }
    }
}
