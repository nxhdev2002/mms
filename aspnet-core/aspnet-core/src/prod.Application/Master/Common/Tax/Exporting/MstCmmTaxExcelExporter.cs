using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Cmm.Dto;
using prod.Master.Cmm.Exporting;
using prod.Storage;

namespace prod.Master.Common.Tax.Exporting
{
    public class MstCmmTaxExcelExporter : NpoiExcelExporterBase, IMstCmmTaxExcelExporter
    {
        public MstCmmTaxExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmTaxDto> tax)
        {
            return CreateExcelPackage(
                "MasterCmmTax.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Tax");
                    AddHeader(
                                sheet,
                                "Code",
                                "Description",
                                "Rate"

                               );
                    AddObjects(
                         sheet, tax,
                                _ => _.Code,
                                _ => _.Description,
                                _ => _.Rate

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
