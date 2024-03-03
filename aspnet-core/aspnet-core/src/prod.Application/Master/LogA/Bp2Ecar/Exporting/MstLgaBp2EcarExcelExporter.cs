using Abp.Application.Services.Dto;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogA.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.LogA.Exporting
{
    public  class MstLgaBp2EcarExcelExporter : NpoiExcelExporterBase, IMstLgaBp2EcarExcelExporter
    {
        public MstLgaBp2EcarExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }



        public FileDto ExportToFile(List<MstLgaBp2EcarDto> bp2ecar) => CreateExcelPackage(
                "MasterLogABp2Ecar.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Bp2Ecar");
                    AddHeader(
                        sheet,
                        ("Code"),
                        ("EcarName"),
                        ("ProdLine"),
                        ("EcarType"),
                        ("Sorting"),
                        ("IsActive")
                    );
                    AddObjects(
                        sheet, bp2ecar,
                        _ => _.Code,
                        _ => _.EcarName,
                        _ => _.ProdLine,
                        _ => _.EcarType,
                        _ => _.Sorting,
                        _ => _.IsActive
                    );
                
                }
            );

    }
}
