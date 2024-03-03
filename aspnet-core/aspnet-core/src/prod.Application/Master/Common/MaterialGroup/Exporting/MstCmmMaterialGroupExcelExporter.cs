using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Dto;
using prod.Storage;
using prod.Master.Common.Dto;
namespace prod.Master.Common.Exporting
{
    public class MstCmmMaterialGroupExcelExporter : NpoiExcelExporterBase, IMstCmmMaterialGroupExcelExporter
    {
        public MstCmmMaterialGroupExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmMaterialGroupDto> materialgroup)
        {
            return CreateExcelPackage(
                "MasterCommonMaterialGroup.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("MaterialGroup");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, materialgroup,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.IsActive

                                );
                });

        }
    }
}
