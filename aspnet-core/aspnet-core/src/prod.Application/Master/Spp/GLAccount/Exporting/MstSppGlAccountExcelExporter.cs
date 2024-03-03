using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Spp.Exporting;
using prod.Master.Spp.Dto;
using prod.Storage;
using prod.Master.Spp.Dto;
namespace prod.Master.Spp.Exporting
{
    public class MstSppGlAccountExcelExporter : NpoiExcelExporterBase, IMstSppGlAccountExcelExporter
    {
        public MstSppGlAccountExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstSppGlAccountDto> glaccount)
        {
            return CreateExcelPackage(
                "MasterSppGlAccount.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GlAccount");
                    AddHeader(
                                sheet,
                                ("GlAccountNo"),
                                ("GlAccountNoS4"),
                                ("GlType"),
                                ("StartDate"),
                                ("EndDate"),
                                ("GlDescEn"),
                                ("GlDesc"),
                                ("CrDb")

                               );
                    AddObjects(
                         sheet, glaccount,
                                _ => _.GlAccountNo,
                                _ => _.GlAccountNoS4,
                                _ => _.GlType,
                                _ => _.StartDate,
                                _ => _.EndDate,
                                _ => _.GlDescEn,
                                _ => _.GlDesc,
                                _ => _.CrDb

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
