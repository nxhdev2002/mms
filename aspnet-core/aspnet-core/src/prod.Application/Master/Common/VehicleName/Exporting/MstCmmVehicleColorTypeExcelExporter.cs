using NPOI.SS.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Master.Common.Exporting
{
    public class MstCmmVehicleNameExcelExporter : NpoiExcelExporterBase, IMstCmmVehicleNameExcelExporter
    {
        public MstCmmVehicleNameExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmVehicleNameDto> vehicleName)
        {
            return CreateExcelPackage(
                "MasterCommonVehicleName.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("VehicleName");
                    AddHeader(
                                sheet,
                                    ("Code"),
                                    ("Name")
                                   );
                    AddObjects(
                         sheet, vehicleName,
                                _ => _.Code,
                                _ => _.Name
                                );
                });

        }
    }
}