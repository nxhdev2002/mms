using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Painting.Dto;
using prod.Master.Painting.Exporting;
using prod.Storage;
using System.Collections.Generic;
namespace prod.Master.Painting.Exporting
{
    public class MstPtsInventoryStdExcelExporter : NpoiExcelExporterBase, IMstPtsInventoryStdExcelExporter
    {
        public MstPtsInventoryStdExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstPtsInventoryStdDto> inventorystd)
        {
            return CreateExcelPackage(
                "MasterPaintingInventoryStd.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InventoryStd");
                    AddHeader(
                                sheet,
                                ("Model"),
                                    ("InventoryStd"),
                                    ("IsActive")
                                   );
                    AddObjects(
                         sheet, inventorystd,
                                _ => _.Model,
                                _ => _.InventoryStd,
                                _ => _.IsActive

                                );
                });

        }
    }
}

