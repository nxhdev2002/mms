using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Master.Common.Exporting
{
    public class MstCmmValuationClassExcelExporter : NpoiExcelExporterBase, IMstCmmValuationClassExcelExporter
    {
        public MstCmmValuationClassExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmValuationClassDto> valuationclass)
        {
            return CreateExcelPackage(
                "MasterCommonValuationClass.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ValuationClass");
                    AddHeader(
                                sheet,
                                ("Code"),
                                    ("Name"),
                                    ("BsAccount"),
                                    ("BsAccountDescription")
                                   );
                    AddObjects(
                         sheet, valuationclass,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.BsAccount,
                    _ => _.BsAccountDescription

                                );
                });

        }
    }
}
