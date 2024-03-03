using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.CarSeries.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Common.CarSeries.Exporting
{
    public class MstCmmCarSeriesExcelExporter : NpoiExcelExporterBase, IMstCmmCarSeriesExcelExporter
    {
        public MstCmmCarSeriesExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmCarSeriesDto> carseries)
        {
            return CreateExcelPackage(
                "MstCmmCarSeries.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CarSeries");
                    AddHeader(
                                sheet,
                                ("Code"),
                                    ("Name")
                                   );
                    AddObjects(
                         sheet, carseries,
                                _ => _.Code,
                                _ => _.Name

                                );
                });

        }
    }
}
