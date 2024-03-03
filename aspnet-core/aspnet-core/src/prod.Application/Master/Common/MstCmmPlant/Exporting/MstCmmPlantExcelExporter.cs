using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Dto;
using prod.Storage;
using prod.Master.Common.Dto;
namespace prod.Master.Common.Exporting
{
    public class MstCmmPlantExcelExporter : NpoiExcelExporterBase, IMstCmmPlantExcelExporter
    {
        public MstCmmPlantExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmPlantDto> plant)
        {
            return CreateExcelPackage(
                "MasterCommonPlant.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Plant");
                    AddHeader(
                                sheet,
                                ("PlantCode"),
                                ("PlantName"),
                                ("BranchNo"),
                                ("AddressLanguageEn"),
                                ("AddressLanguageVn")
                                

                               );
                    AddObjects(
                         sheet, plant,
                                _ => _.PlantCode,
                                _ => _.PlantName,
                                _ => _.BranchNo,
                                _ => _.AddressLanguageEn,
                                _ => _.AddressLanguageVn
                                

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
