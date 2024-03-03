using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Dto;
using prod.Storage;
using prod.Master.Common.Dto;
using NPOI.SS.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Storage;

namespace prod.Master.Common.Exporting
{
    public class MstCmmMaterialTypeExcelExporter : NpoiExcelExporterBase, IMstCmmMaterialTypeExcelExporter
    {
        public MstCmmMaterialTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmMaterialTypeDto> materialtype)
        {
            return CreateExcelPackage(
                "MasterCommonMaterialType.xlsx",
                excelPackage =>
                {
                var sheet = excelPackage.CreateSheet("MaterialType");
                AddHeader(
                            sheet,
                            ("Code"),
								("Name"),
								("IsActive")
							   );
            AddObjects(
                 sheet, materialtype,
                        _ => _.Code,
            _ => _.Name,
                        _ => _.IsActive

                        );
        });

        }
}
}
