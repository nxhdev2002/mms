using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.PIO.PartListInl.Dto;
using prod.Inventory.PIO.PartListInl.Export;
using prod.Inventory.PIO.PartListOff.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.PartListOff.Export
{
    public class InvPioPartListOffExcelExporter : NpoiExcelExporterBase, IInvPioPartListOffExcelExporter
    {
        public InvPioPartListOffExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }


        public FileDto ExportValidateToFile(List<ValidatePartListDto> invckdpartlistvalidate)
        {
            return CreateExcelPackage(
                "InventoryPIOPartListOffValidate.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartList");
                    AddHeader(
                                sheet,
                                ("PartListId"),
                                ("ErrorDescription"),
                                ("PartNo"),
                                ("PartNoNormalizedS4"),
                                ("PartName"),
                                ("Model"),
                                ("Cfc"),
                                ("MaterialId"),
                                ("OrderPattern"),
                                ("Grade"),
                                ("Shop"),
                                ("BodyColor"),
                                ("UsageQty"),
                                ("StartLot"),
                                ("EndLot"),
                                ("StartRun"),
                                ("EndRun")

                               );
                    AddObjects(
                         sheet, invckdpartlistvalidate,
                             _ => _.PartListId,
                             _ => _.ErrorDescription,
                             _ => _.PartNo,
                             _ => _.PartNoNormalizedS4,
                             _ => _.PartName,
                             _ => _.Model,
                             _ => _.Cfc,
                             _ => _.MaterialId,
                             _ => _.OrderPattern,
                             _ => _.Grade,
                             _ => _.Shop,
                             _ => _.BodyColor,
                             _ => _.UsageQty,
                             _ => _.StartLot,
                             _ => _.EndLot,
                             _ => _.StartRun,
                             _ => _.EndRun

                                );


                });

        }

        public FileDto ExportToFileErr(List<ImportPioPartListOffDto> errpartlist)
        {
            return CreateExcelPackage(
                "ListErrorImportPioPartListInl.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartListError");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Model"),
                                ("Shop"),
                                ("IdLine"),
                                ("PartNo"),
                                ("PartName"),
                                ("SupplierNo"),
                                ("SupplierCd"),
                                ("BodyColor"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, errpartlist,
                                _ => _.ROW_NO,
                                _ => _.Model,
                                _ => _.Shop,
                                _ => _.IdLine,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.SupplierNo,
                                _ => _.SupplierCd,
                                _ => _.BodyColor,
                                _ => _.ErrorDescription
                                );


                });

        }

    }
}
