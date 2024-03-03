using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Dto;
using prod.Storage;
using prod.Master.Common.Dto;
namespace prod.Master.Common.Exporting
{
    public class MstCmmStorageLocationExcelExporter : NpoiExcelExporterBase, IMstCmmStorageLocationExcelExporter
    {
        public MstCmmStorageLocationExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmStorageLocationDto> storagelocation)
        {
            return CreateExcelPackage(
                "MasterCommonStorageLocation.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StorageLocation");
                    AddHeader(
                                sheet,
                                ("PlantCode"),
                                ("PlantName"),
                                ("StorageLocation"),
                                ("StorageLocationName"),
                                ("AddressLanguageEn"),
                                ("AddressLanguageVn"),
                                ("CategoryId"),
                                ("Category"),
                                 ("IsActive")


                               );
                    AddObjects(
                         sheet, storagelocation,
                                _ => _.PlantCode,
                                _ => _.PlantName,
                                _ => _.StorageLocation,
                                _ => _.StorageLocationName,
                                _ => _.AddressLanguageEn,
                                _ => _.AddressLanguageVn,
                                _ => _.CategoryId,
                                _ => _.Category,
                                _ => _.IsActive


                                );
                });

        }
    }
}
