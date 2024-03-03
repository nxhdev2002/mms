using System.Collections.Generic;
using vovina.Master.Common.Exporting;
using NPOI.SS.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Storage;

namespace vovina.Master.Common.Exporting
{
    public class MstCmmFuelTypeExcelExporter : NpoiExcelExporterBase, IMstCmmFuelTypeExcelExporter
    {
        public MstCmmFuelTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmFuelTypeDto> fueltype)
        {
            return CreateExcelPackage(
                "MasterCommonFuelType.xlsx",
                excelPackage =>
                {
                var sheet = excelPackage.CreateSheet("FuelType");
                AddHeader(
                            sheet,
                            ("Code"),
								("Name"),
								("IsActive")
							   );
            AddObjects(
                 sheet, fueltype,
                        _ => _.Code,
            _ => _.Name,
                        _ => _.IsActive

                        );
        });

        }
}
}
