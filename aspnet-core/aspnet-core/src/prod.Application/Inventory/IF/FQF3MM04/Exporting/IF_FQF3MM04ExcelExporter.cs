using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Storage;
using prod.Inventory.IF.Exporting;
using prod.Inventory.IF.FQF3MM04.Dto;

namespace prod.IF.IF.Exporting
{
    public class IF_FQF3MM04ExcelExporter : NpoiExcelExporterBase, IIF_FQF3MM04ExcelExporter
    {
        public IF_FQF3MM04ExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<IF_FQF3MM04Dto> fqf3mm04)
        {
            return CreateExcelPackage(
                "IFIFFQF3MM04.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FQF3MM04");
                    AddHeader(
                                sheet,
                                ("RecordId (M)"),
                                ("InvoiceNo (M)"),
                                ("Renban (M)"),
                                ("ContainerNo (M)"),
                                ("ModuleNo (O)"),
                                ("DevaningDate (M)"),
                                ("Plant (M)"),
                                ("CancelFlag (M)"),
                                ("EndOfRecord (M)")

                               );
                    AddObjects(
                         sheet,  fqf3mm04,
                                _ => _.RecordId,
                                _ => _.InvoiceNo,
                                _ => _.Renban,
                                _ => _.ContainerNo,
                                _ => _.ModuleNo,
                                _ => _.DevaningDate,
                                _ => _.Plant,
                                _ => _.CancelFlag,
                                _ => _.EndOfRecord

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }


        public FileDto ExportValidateToFile(List<GetIF_FQF3MM04_VALIDATE> fqf3mm04)
        {
            return CreateExcelPackage(
                "VALIDATEFQF3MM04.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("VALIDATE_FQF3MM04");
                    AddHeader(
                                sheet,
                                 ("ErrorDescription"),
                                ("RecordId (M)"),
                                ("InvoiceNo (M)"),
                                ("Renban (M)"),
                                ("ContainerNo (M)"),
                                ("DevaningDate (M)"),
                                ("Plant (M)"),
                                ("CancelFlag (M)"),
                                ("EndOfRecord (M)"),
                                ("HeaderFwgId"),
                                ("HeaderId"),
                                ("TrailerId")
                            

                               );
                    AddObjects(
                         sheet, fqf3mm04,
                               _ => _.ErrorDescription,
                                _ => _.RecordId,
                                _ => _.InvoiceNo,
                                _ => _.Renban,
                                _ => _.ContainerNo,
                                _ => _.DevaningDate,
                                _ => _.Plant,
                                _ => _.CancelFlag,
                                _ => _.EndOfRecord,
                                _ => _.HeaderFwgId,
                                _ => _.HeaderId,
                                _ => _.TrailerId
                          

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
