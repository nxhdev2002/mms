using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Dto;
using prod.Storage;
using prod.Master.Common.Dto;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Storage;

namespace prod.Master.Common.Exporting
{
    public class MstCmmBrandExcelExporter : NpoiExcelExporterBase, IMstCmmBrandExcelExporter
    {
        public MstCmmBrandExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmBrandDto> brand)
        {
            return CreateExcelPackage(
                "MasterCommonBrand.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Brand");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, brand,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.IsActive

                                );
                });

        }
    }
}
