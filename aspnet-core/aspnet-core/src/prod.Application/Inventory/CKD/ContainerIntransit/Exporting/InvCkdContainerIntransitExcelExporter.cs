using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using prod.Inventory.CKD.Dto;
namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdContainerIntransitExcelExporter : NpoiExcelExporterBase, IInvCkdContainerIntransitExcelExporter
    {
        public InvCkdContainerIntransitExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdContainerIntransitDto> containerintransit)
        {
            return CreateExcelPackage(
                "InventoryCKDContainerIntransit.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerIntransit");
                    AddHeader(
                                sheet,
                                ("ContainerNo"),
                                ("Renban"),
                                ("SupplierNo"),
                                ("ETD"),
                                ("ETA"),
                                ("TransDate"),
                                ("TmvDate"),
                                ("Cif"),
                                ("Tax"),
                                ("Inland"),
                                ("Thc"),
                                ("Fob"),
                                ("Freight"),
                                ("Insurance"),
                                ("Amount"),
                                ("Status"),
                                ("PeriodDate"),
                                ("PeriodId"),
                                ("CifVn"),
                                ("TaxVn"),
                                ("InlandVn"),
                                ("ThcVn"),
                                ("FobVn"),
                                ("FreightVn"),
                                ("InsuranceVn"),
                                ("AmountVn"),
                                ("Forwarder"),
                                ("Generated"),
                                ("Shop")
                               );
                    AddObjects(
                         sheet, containerintransit,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.SupplierNo,
                                _ => _.ShippingDate,
                                _ => _.PortDate,
                                _ => _.TransDate,
                                _ => _.TmvDate,
                                _ => _.Cif,
                                _ => _.Tax,
                                _ => _.Inland,
                                _ => _.Thc,
                                _ => _.Fob,
                                _ => _.Freight,
                                _ => _.Insurance,
                                _ => _.Amount,
                                _ => _.Status,
                                _ => _.PeriodDate,
                                _ => _.PeriodId,
                                _ => _.CifVn,
                                _ => _.TaxVn,
                                _ => _.InlandVn,
                                _ => _.ThcVn,
                                _ => _.FobVn,
                                _ => _.FreightVn,
                                _ => _.InsuranceVn,
                                _ => _.AmountVn,
                                _ => _.Forwarder,
                                _ => _.Generated,
                                _ => _.Shop
                                );
                });

        }
    }
}
