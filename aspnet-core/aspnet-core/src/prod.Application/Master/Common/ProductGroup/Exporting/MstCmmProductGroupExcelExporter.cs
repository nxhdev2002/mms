using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Storage;
using System.Collections.Generic;
namespace vovina.Master.Common.Exporting
{
    public class MstCmmProductGroupExcelExporter : NpoiExcelExporterBase, IMstCmmProductGroupExcelExporter
    {
        public MstCmmProductGroupExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmProductGroupDto> productGroup)
        {
            return CreateExcelPackage(
                "MasterCommonProductGroup.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ProductGroup");
                    AddHeader(
                                sheet,
                                ("Code"),
                                    ("Description")
                                   );
                    AddObjects(
                         sheet, productGroup,
                                _ => _.Code,
                                _ => _.Description

                                );
                });

        }
    }
}
