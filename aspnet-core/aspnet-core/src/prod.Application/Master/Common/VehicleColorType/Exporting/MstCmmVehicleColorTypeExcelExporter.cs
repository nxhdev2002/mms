using NPOI.SS.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Master.Common.Exporting
{
    public class MstCmmVehicleColorTypeExcelExporter : NpoiExcelExporterBase, IMstCmmVehicleColorTypeExcelExporter
    {
        public MstCmmVehicleColorTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmVehicleColorTypeDto> vehiclecolortype)
        {
            return CreateExcelPackage(
                "MasterCommonVehicleColorType.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("VehicleColorType");
                    AddHeader(
                                sheet,
                                    ("Code"),
                                    ("Name")
                                   );
                    AddObjects(
                         sheet, vehiclecolortype,
                                _ => _.Code,
                                _ => _.Name
                                );
                });

        }
    }
}