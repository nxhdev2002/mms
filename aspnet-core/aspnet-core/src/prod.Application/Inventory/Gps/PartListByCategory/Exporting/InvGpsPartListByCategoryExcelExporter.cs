using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.Gps.PartListByCategory.Dto;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.PartListByCategory.Exporting
{

        public class InvGpsPartListByCategoryExcelExporter : NpoiExcelExporterBase, IInvGpsPartListByCategoryExcelExporter
        {
            public InvGpsPartListByCategoryExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
            public FileDto ExportToFile(List<InvGpsPartListByCategoryDto> invgpsmaster)
            {
                return CreateExcelPackage(
                    "InvGpsPartListByCategory.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet("InvGpsPartListByCategory");
                        AddHeader(
                                    sheet,
                                    ("Part No"),
                                    ("Description"),
                                    ("Uom"),
                                    ("Category"),
                                    ("Location"),
                                    ("ExpenseAccount"),
                                    ("Group"),
                                    ("CurrentCategory"),
                                    ("PartType")
                                   );
                        AddObjects(
                             sheet, invgpsmaster,
                                    _ => _.Item,
                                    _ => _.Description,
                                    _ => _.Uom,
                                    _ => _.Category,
                                    _ => _.Location,
                                    _ => _.ExpenseAccount,
                                    _ => _.Group,
                                    _ => _.CurrentCategory,
                                    _ => _.PartType

                                    );
                    });

            }
        }
    }

