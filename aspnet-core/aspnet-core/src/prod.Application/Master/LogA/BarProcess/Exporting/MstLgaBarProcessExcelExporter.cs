using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Master.LogA.Exporting;
using prod.Dto;
using prod.Storage;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA.Exporting
{
    public class MstLgaBarProcessExcelExporter : NpoiExcelExporterBase, IMstLgaBarProcessExcelExporter
    {
        public MstLgaBarProcessExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstLgaBarProcessDto> barprocess)
        {
            return CreateExcelPackage(
                "MasterLogABarProcess.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("BarProcess");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("ProcessName"),
                                ("ProcessGroup"),
                                ("ProcessSubgroup"),
                                ("ProdLine"),
                                ("ProcessType"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, barprocess,
                                _ => _.Code,
                                _ => _.ProcessName,
                                _ => _.ProcessGroup,
                                _ => _.ProcessSubgroup,
                                _ => _.ProdLine,
                                _ => _.ProcessType,
                                _ => _.IsActive

                                );
                });

        }
    }
}