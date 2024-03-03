using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Plm.Exporting;
using prod.Master.Plm.Dto;
using prod.Storage;
using prod.Master.Plm.Dto;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Plm.Dto;
using prod.Master.Plm.Exporting;
using prod.Storage;

namespace prod.Master.Plm.Exporting
{
    public class MstPlmLotCodeGradeExcelExporter : NpoiExcelExporterBase, IMstPlmLotCodeGradeExcelExporter
    {
        public MstPlmLotCodeGradeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstPlmLotCodeGradeDto> lotcodegrade)
        {
            return CreateExcelPackage(
                "MasterPlmLotCodeGrade.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("LotCodeGrade");
                    AddHeader(
                                sheet,
                                ("ModelId"),
                                ("LotCode"),
                                ("Cfc"),
                                ("Grade"),
                                ("Odering"),
                                ("GradeName"),
                                ("ModeCode"),
                                ("ModelVin"),
                                ("VisStart"),
                                ("VisEnd"),
                                ("MaLotCode"),
                                ("VehicleId"),
                                ("TestNo"),
                                ("Dab"),
                                ("Pab"),
                                ("EngCode"),
                                ("Lab"),
                                ("Rab"),
                                ("Kab"),
                                ("IsFcLabel"),
                                ("IsActive"),
                                ("R"),
                                ("G"),
                                ("B"),
                                ("Clab"),
                                ("CharStr")

                               );
                    AddObjects(
                         sheet, lotcodegrade,
                                _ => _.ModelId,
                                _ => _.LotCode,
                                _ => _.Cfc,
                                _ => _.Grade,
                                _ => _.Odering,
                                _ => _.GradeName,
                                _ => _.ModeCode,
                                _ => _.ModelVin,
                                _ => _.VisStart,
                                _ => _.VisEnd,
                                _ => _.MaLotCode,
                                _ => _.VehicleId,
                                _ => _.TestNo,
                                _ => _.Dab,
                                _ => _.Pab,
                                _ => _.EngCode,
                                _ => _.Lab,
                                _ => _.Rab,
                                _ => _.Kab,
                                _ => _.IsFcLabel,
                                _ => _.IsActive,
                                _ => _.R,
                                _ => _.G,
                                _ => _.B,
                                _ => _.Clab,
                                _ => _.CharStr

                                );
                });

        }
    }
}
