using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Dto;
using prod.Storage;
using prod.Master.Common.Dto;
namespace prod.Master.Common.Exporting
{
    public class MstCmmStorageLocationCategoryExcelExporter : NpoiExcelExporterBase, IMstCmmStorageLocationCategoryExcelExporter
    {
        public MstCmmStorageLocationCategoryExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmStorageLocationCategoryDto> storagelocationcategory)
        {
            return CreateExcelPackage(
                "MasterCommonStorageLocationCategory.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StorageLocationCategory");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name"),
                                ("AreaType")
                               );
                    AddObjects(
                         sheet, storagelocationcategory,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.AreaType
                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
