using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.HrTitles.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.HrTitles.Exporting
{
    public class MstInvHrTitlesExcelExporter : NpoiExcelExporterBase, IMstInvHrTitlesExcelExporter
    {
        public MstInvHrTitlesExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<MstInvHrTitlesDto> mstinvhrtitles)
        {
            return CreateExcelPackage(
                "MasterInvHrTitles.xlsx",
            excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("MstInvHrTitles");
                AddHeader(
                            sheet,
                            ("Id"),
                            ("Code"),
                            ("Name"),
                            ("Description"),
                            ("Is Active")
                        );
                AddObjects(
                    sheet, mstinvhrtitles,
                    _ => _.Id_Code,
                        _ => _.Code,
                        _ => _.Name,
                        _ => _.Description,
                        _ => _.IsActive
                        );
            });
        }
    }
}
