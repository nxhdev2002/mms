using System.Collections.Generic;
using prod.Master.LogA.Dto;
using prod.Storage;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;

namespace prod.Master.LogA.Exporting
{
    public class MstLgaEkbProcessExcelExporter : NpoiExcelExporterBase, IMstLgaEkbProcessExcelExporter
    {
        public MstLgaEkbProcessExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstLgaEkbProcessDto> ekbprocess)
        {
            return CreateExcelPackage(
                "MasterLogAEkbProcess.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("EkbProcess");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("ProcessName"),
                                ("ProcessGroup"),
                                ("ProcessSubgroup"),
                                ("ProdLine"),
                                ("LeadTime"),
                                ("Sorting"),
                                ("ProcessType"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, ekbprocess,
                                _ => _.Code,
                                _ => _.ProcessName,
                                _ => _.ProcessGroup,
                                _ => _.ProcessSubgroup,
                                _ => _.ProdLine,
                                _ => _.LeadTime,
                                _ => _.Sorting,
                                _ => _.ProcessType,
                                _ => _.IsActive

                                );
                });

        }
    }
}
