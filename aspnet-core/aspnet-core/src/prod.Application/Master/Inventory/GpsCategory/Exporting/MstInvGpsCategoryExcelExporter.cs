using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.DriveTrain.Dto;
using prod.Master.Inventory.GpsCategory.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.GpsCategory.Exporting
{
    public class MstInvGpsCategoryExcelExporter : NpoiExcelExporterBase, IMstInvGpsCategoryExcelExporter
    {
        public MstInvGpsCategoryExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvGpsCategoryDto> gpscategory)
        {
            return CreateExcelPackage(
                "MstInvGpsCategory.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsCateGory");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name")
                                   );
                    AddObjects(
                         sheet, gpscategory,
                                _ => _.Code,
                                _ => _.Name

                                );
                });
        }
    }
}
