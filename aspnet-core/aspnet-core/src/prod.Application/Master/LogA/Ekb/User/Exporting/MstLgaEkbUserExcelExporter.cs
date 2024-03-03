using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogA.Dto;
using prod.Storage;

namespace prod.Master.LogA.Exporting
{
    public class MstLgaEkbUserExcelExporter : NpoiExcelExporterBase, IMstLgaEkbUserExcelExporter
    {
        public MstLgaEkbUserExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstLgaEkbUserDto> ekbuser)
        {
            return CreateExcelPackage(
                "MasterLogAEkbUser.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("EkbUser");
                    AddHeader(
                                sheet,
                                ("UserCode"),
                                ("UserName"),
                                ("ProcessId"),
                                ("ProcessCode"),
                                ("ProcessGroup"),
                                ("ProcessSubgroup"),
                                ("ProdLine"),
                                ("UserType"),
                                ("LeadTime"),
                                ("Sorting"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet,ekbuser,
                                _ => _.UserCode,
                                _ => _.UserName,
                                _ => _.ProcessId,
                                _ => _.ProcessCode,
                                _ => _.ProcessGroup,
                                _ => _.ProcessSubgroup,
                                _ => _.ProdLine,
                                _ => _.UserType,
                                _ => _.LeadTime,
                                _ => _.Sortingg,
                                _ => _.IsActive
                                );
                });

        }
    }
}
