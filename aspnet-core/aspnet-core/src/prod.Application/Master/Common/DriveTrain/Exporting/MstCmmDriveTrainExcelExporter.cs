using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.DriveTrain.Dto;
using prod.Master.Common.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Common.DriveTrain.Exporting
{
    public class MstCmmDriveTrainExcelExporter : NpoiExcelExporterBase, IMstCmmDriveTrainExcelExporter
    {
        public MstCmmDriveTrainExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmDriveTrainDto> drivetrain)
        {
            return CreateExcelPackage(
                "MstCmmDriveTrain.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("DriveTrain");
                    AddHeader(
                                sheet,
                                ("Code"),
                                    ("Name")
                                   );
                    AddObjects(
                         sheet, drivetrain,
                                _ => _.Code,
                                _ => _.Name

                                );
                });
        }
    }
}
