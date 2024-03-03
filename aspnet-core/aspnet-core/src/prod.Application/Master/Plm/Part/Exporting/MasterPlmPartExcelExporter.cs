using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Plm.Exporting;
using prod.Master.Plm.Dto;
using prod.Storage;
using prod.Master.Plm.Dto;
using NPOI.SS.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Plm.Dto;
using prod.Master.Plm.Exporting;
using prod.Master.Plm;
using prod.Storage;

namespace prod.Master.Plm.Exporting
{
    public class MasterPlmPartExcelExporter : NpoiExcelExporterBase, IMasterPlmPartExcelExporter
    {
        public MasterPlmPartExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MasterPlmPartDto> part)
        {
            return CreateExcelPackage(
                "MasterPlmPart.xlsx",
                excelPackage =>
                {
                var sheet = excelPackage.CreateSheet("Part");
                AddHeader(
                    sheet,
                            ("PartName "),
								("PartCd "),
								("R "),
								("G "),
								("B ")
							   );
            AddObjects(
                 sheet, part,
                        _ => _.PartName,
                        _ => _.PartCd,
            _ => _.R,
                        _ => _.G,
                        _ => _.B

                        );
        });

        }
}
}