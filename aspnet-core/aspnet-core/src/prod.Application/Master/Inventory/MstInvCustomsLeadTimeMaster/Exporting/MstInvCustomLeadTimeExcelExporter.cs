using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.Exporting
{
    public class MstInvCustomsLeadTimeExcelExporter : NpoiExcelExporterBase, IMstInvCustomsLeadTimeExcelExporter
    {
        public MstInvCustomsLeadTimeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvCustomsLeadTimeDto> CustomsLeadTimeMst)
        {
            return CreateExcelPackage(
                "MstInvCustomsLeadTimeMaster.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CustomsLeadTimeMst");
                    AddHeader(
                                sheet,
                                ("Exporter"),
                                ("Carrier"),
                                ("Leadtime")
      
                               );
                    AddObjects(
                         sheet, CustomsLeadTimeMst,
                                _ => _.Exporter,
                                _ => _.Carrier,
                                _ => _.Leadtime
                                );


                });

        }
    }
}
