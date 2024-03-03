using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.CKD.Exporting;
using prod.Master.Inventory.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.Exporting
{
    public class MstGpsMaterialRegisterByShopExcelExporter : NpoiExcelExporterBase, IMstGpsMaterialRegisterByShopExcelExporter
    {
        public MstGpsMaterialRegisterByShopExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstGpsMaterialRegisterByShopDto> mstGpsMaterialRegisterByShop)
        {
            return CreateExcelPackage(
                "MstGpsMaterialRegisterByShop.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("MstGpsMaterialRegisterByShop");
                    AddHeader(
                                sheet,
                                    ("PartNo"),
                                    ("Description"),
                                    ("Uom"),
                                    ("Part Type"),
                                    ("Expense Account"),
                                    ("Shop"),
                                    ("Cost Center")
                               );
                    AddObjects(
                         sheet, mstGpsMaterialRegisterByShop,
                                    _ => _.PartNo,
                                    _ => _.Description,
                                    _ => _.Uom,
                                    _ => _.Category,
                                    _ => _.ExpenseAccount,
                                    _ => _.Shop,
                                    _ => _.CostCenter
                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
