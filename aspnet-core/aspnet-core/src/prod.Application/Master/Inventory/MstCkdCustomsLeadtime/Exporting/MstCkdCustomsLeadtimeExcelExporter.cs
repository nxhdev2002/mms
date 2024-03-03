using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.CKD.Exporting;
using prod.Master.CKD.Dto;
using prod.Storage;
using prod.Master.CKD.Dto;
namespace prod.Master.CKD.Exporting
{
    public class MstCkdCustomsLeadtimeExcelExporter : NpoiExcelExporterBase, IMstCkdCustomsLeadtimeExcelExporter
    {
        public MstCkdCustomsLeadtimeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCkdCustomsLeadtimeDto> customsleadtime)
        {
            return CreateExcelPackage(
                "MasterCKDCustomsLeadtime.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CustomsLeadtime");
                    AddHeader(
                                sheet,
                                ("SupplierNo"),
                                ("Leadtime")


                               );
                    AddObjects(
                         sheet, customsleadtime,
                                _ => _.SupplierNo,
                                _ => _.Leadtime



                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
