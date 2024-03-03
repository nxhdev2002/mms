

using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.MstInvHrEmployee.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Master.Inventory.MstInvHrEmployee.Exporting
{
    public class MstInvHrEmployeeExcelExporter : NpoiExcelExporterBase, IMstInvHrEmployeeExcelExporter
    {
        public MstInvHrEmployeeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {}
        public FileDto ExportEmployeeToFile(List<MstInvHrEmployeeDto> mstInvHrEmployee)
        {
            return CreateExcelPackage(
                "MstInvHrEmployee.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet("InvHrEmployee");
                        AddHeader(
                                    sheet,
                                    ("EmployeeCode"),
                                    ("UserName"),
                                    ("EmailAddress"),
                                    ("TitleName"),
                                    ("PositionName"),
                                    ("OrgStructureName"),
                                    ("IsActive")
                                   );
                        AddObjects(
                             sheet, mstInvHrEmployee,
                                    _ => _.EmployeeCode,
                                    _ => _.UserName,
                                    _ => _.EmailAddress,
                                    _ => _.TitleName,
                                    _ => _.PositionName,
                                    _ => _.OrgStructureName,
                                    _ => _.IsActive

                                    );
                    });

        }

    }
}
