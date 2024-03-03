using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Dto;
using prod.Storage;
using prod.Master.Common.Dto;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Storage;

namespace prod.Master.Common.Exporting
{
    public class MstCmmShopTypeExcelExporter : NpoiExcelExporterBase, IMstCmmShopTypeExcelExporter
    {
        public MstCmmShopTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmShopTypeDto> shoptype)
        {
            return CreateExcelPackage(
                "MasterCommonShopType.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ShopType");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, shoptype,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.IsActive

                                );
                });

        }
    }
}
