using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.Gps.Mapping.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.Gps.Mapping.Exporting
{
    public class InvGpsMappingExcelExporter : NpoiExcelExporterBase, IInvGpsMappingExcelExporter
    {
        public InvGpsMappingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<InvGpsMappingDto> data)
        {
            return CreateExcelPackage(
                "InvGpsMapping.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvGpsMapping");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartType"),
                                ("PartCatetory"),
                                ("ShopRegister"),
                                ("Location"),
                                ("CostCenter"),
                                ("Wbs"),
                                ("GlAccount"),
                                ("ExpenseAccount"),
                                ("EffectiveDateFrom"),
                                ("EffectiveDateTo"),
                                ("LastRenew"),
                                ("RenewBy"),
                                ("Status")
                               );
                    AddObjects(
                         sheet, data,
                                _ => _.PartNo,
                                _ => _.PartType,
                                _ => _.PartCatetory,
                                _ => _.ShopRegister,
                                _ => _.Location,
                                _ => _.CostCenter,
                                _ => _.Wbs,
                                _ => _.GlAccount,
                                _ => _.ExpenseAccount,
                                _ => _.EffectiveDateFrom,
                                _ => _.EffectiveDateTo,
                                _ => _.LastRenew,
                                _ => _.RenewBy,
                                _ => _.Status
                                );

                });
        }
    }
}
