using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.GpsCategory.Exporting;
using prod.Master.Inventory.GpsMaterialCategory.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.GpsMaterialCategory.Exporting
{
    public class MstInvGpsMaterialCategoryExcelExporter : NpoiExcelExporterBase, IMstInvGpsMaterialCategoryExcelExporter
    {
        public MstInvGpsMaterialCategoryExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvGpsMaterialCategoryDto> mstinvgpsmaterialcategory)
        {
            return CreateExcelPackage(
                           "MstInvGpsMaterialCategory.xlsx",
                           excelPackage =>
                           {
                               var sheet = excelPackage.CreateSheet("GpsMaterialCategory");
                               AddHeader(
                                           sheet,
                                           ("Name"),
                                           ("IsActive")
                                              );
                               AddObjects(
                                    sheet, mstinvgpsmaterialcategory,
                                           _ => _.Name,
                                           _ => _.IsActive

                                           );
                           });
        }
    }
}