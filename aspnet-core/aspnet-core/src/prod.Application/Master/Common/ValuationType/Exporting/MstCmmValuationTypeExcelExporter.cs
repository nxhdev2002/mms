using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Dto;
using prod.Storage;


namespace prod.Master.Common.Exporting
{
    public class MstCmmValuationTypeExcelExporter : NpoiExcelExporterBase, IMstCmmValuationTypeExcelExporter
    {
        public MstCmmValuationTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmValuationTypeDto> valuationtype)
        {
            return CreateExcelPackage(
                "MasterCommonValuationType.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ValuationType");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name")
                              );
                    AddObjects(
                         sheet, valuationtype,
                                _ => _.Code,
                                _ => _.Name
                              );
                });

        }
    }
}
