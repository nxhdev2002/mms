using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Storage;
using prod.Dto;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Exporting;

namespace prod.Master.LogA.Exporting
{
    public class MstLgaBarUserExcelExporter : NpoiExcelExporterBase, IMstLgaBarUserExcelExporter
    {
        public MstLgaBarUserExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstLgaBarUserDto> baruser)
        {
            return CreateExcelPackage(
                "MasterLogABarUser.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("BarUser");
                    AddHeader(
                                sheet,
                                ("UserId"),
                                ("UserName"),
                                ("UserDescription"),
                                ("IsNeedPass"),
                                ("Pwd"),
                                ("ProcessId"),
                                ("ProcessCode"),
                                ("ProcessGroup"),
                                ("ProcessSubgroup"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet, baruser,
                                _ => _.UserId,
                                _ => _.UserName,
                                _ => _.UserDescription,
                                _ => _.IsNeedPass,
                                _ => _.Pwd,
                                _ => _.ProcessId,
                                _ => _.ProcessCode,
                                _ => _.ProcessGroup,
                                _ => _.ProcessSubgroup,
                                _ => _.IsActive
                                );
                });

        }
    }
}
