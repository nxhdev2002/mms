using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.Gps.User.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.Gps.User.Exporting
{
    public class InvGpsUserExcelExporter : NpoiExcelExporterBase, IInvGpsUserExcelExporter
    {
        public InvGpsUserExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<InvGpsUserDto> data)
        {
            return CreateExcelPackage(
                "InvGpsUser.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvGpsUser");
                    AddHeader(
                                sheet,
                                ("Employee Code"),
                                ("Name"),
                                ("Shop"),
                                ("Team"),
                                ("Cost Center"),
                                ("Group"),
                                ("Sub Group"),
                                ("Division"),
                                ("Dept")
                               );
                    AddObjects(
                         sheet, data,
                                _ => _.EmployeeCode,
                                _ => _.Name,
                                _ => _.Shop,
                                _ => _.Team,
                                _ => _.CostCenter,
                                _ => _.Group,
                                _ => _.SubGroup,
                                _ => _.Division,
                                _ => _.Dept
                                
                                );

                });
        }
    }
}

