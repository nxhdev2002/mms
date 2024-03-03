using NPOI.OpenXmlFormats.Dml.Spreadsheet;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.PIO.PartList.Dto;
using prod.Inventory.PIO.PartListOff.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.PartList.Exporting
{
    public class InvPioPartListExcelExporter : NpoiExcelExporterBase, IInvPioPartListExcelExporter
    {
        public InvPioPartListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFileErr(List<InvPioPartListImportDto> errpartlist)
        {
            return CreateExcelPackage(
                "ListErrorImportPioPartList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartListError");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Type"),
                                ("Model"),
                                ("PartNo"),
                                ("PartName"),
                                ("ECIPart"),
                                ("BoxSize"),
                                ("StartDate"),
                                ("EndDate"),
                                ("MktCode"),
                                ("Qty"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, errpartlist,
                                _ => _.ROW_NO,
                                _ => _.Type,
                                _ => _.Model,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.ECIPart,
                                _ => _.BoxSize,
                                _ => _.StartDate,
                                _ => _.EndDate,
                                _ => _.MktCode,
                                _ => _.Qty,
                                _ => _.ErrorDescription
                                );


                });

        }

        public FileDto ExportToFile(List<InvPioPartListDto> partlist)
        {
            return CreateExcelPackage(
                "InventoryPIOPartList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartList");
                    AddHeader(
                                sheet,
                                ("FullModel"),
                                ("ProdSfx"),
                                ("Marketing Code"),
                                ("PartNo"),
                                ("PartName"),
                                ("PartType"),
                                ("PartTypeDescription"),
                                ("PioType"),
                                ("BoxSize"),
                                ("StartDate"),
                                ("EndDate"),
                                ("Remark"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, partlist,
                                _ => _.FullModel,
                                _ => _.ProdSfx,
                                _ => _.MktCode,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.PartType,
                                _ => _.PartDescription,
                                _ => _.PioType,
                                _ => _.BoxSize,
                                _ => _.StartDate,
                                _ => _.EndDate,
                                _ => _.Remark,
                                _ => _.IsActive

                                );

                    for (var i = 0; i < 9; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
