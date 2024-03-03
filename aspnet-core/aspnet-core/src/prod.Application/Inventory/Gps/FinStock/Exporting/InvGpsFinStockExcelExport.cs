using NPOI.SS.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.Gps.FinStock.Dto;
using prod.Inventory.GPS;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.FinStock.Exporting
{
    public class InvGpsFinStockExcelExport : NpoiExcelExporterBase, IInvGpsFinStockExcelExport
    {
        public InvGpsFinStockExcelExport(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<InvGpsFinStockDto> invgpsfinstock)
        {
            return CreateExcelPackage(
                "InventoryGPSFinStock.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvGpsFinStock");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartName"),
                                ("Qty"),
                                ("Price"),
                               // ("WorkingDate"),
                               // ("TransactionId"),
                               // ("Dock"),
                                ("Location")
                               );
                    AddObjects(
                         sheet, invgpsfinstock,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Qty,
                                _ => _.Price,
                                //_ => _.WorkingDate,
                              //  _ => _.TransactionId,
                              //  _ => _.Dock,
                                _ => _.Location
                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

               
        }

        public FileDto ExportToFileLotErr(List<InvGpsFinStockImportDto> invgpsfinstockerr)
        {
            return CreateExcelPackage(
                "ListErrorImportGpsFinStock.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FinStockError");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("PartNo"),
                                ("Qty"),
                                ("Location"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, invgpsfinstockerr,
                                _ => _.ROW_NO,
                                _ => _.PartNo,
                                _ => _.Qty,
                                _ => _.Location,
                                _ => _.ErrorDescription
                                );


                });

        }
    }
}
