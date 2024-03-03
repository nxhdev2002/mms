using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Storage;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdShipmentExcelExporter : NpoiExcelExporterBase, IInvCkdShipmentExcelExporter
    {
        public InvCkdShipmentExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdShipmentDto> shipment)
        {
            return CreateExcelPackage(
                "InventoryCKDShipment.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Shipment");
                    AddHeader(
                                sheet,
                                ("ShipmentNo"),
                                ("ShippingcompanyCode"),
                                ("SupplierNo"),
                                ("Buyer"),
                                ("FromPort"),
                                ("ToPort"),
                                ("ShipmentDate"),
                                ("Etd"),
                                ("Eta"),
                                ("Ata"),
                                ("FeedervesselName"),
                                ("OceanvesselName"),
                                ("OceanvesselvoyageNo"),
                                ("NoofinvoicewithinshipmentNo"),
                                ("Noof20Ftcontainers"),
                                ("Noof40Ftcontainers"),
                                ("Lclvolume"),
                                ("Atd"),
                                ("Status"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, shipment,
                                _ => _.ShipmentNo,
                                _ => _.ShippingcompanyCode,
                                _ => _.SupplierNo,
                                _ => _.Buyer,
                                _ => _.FromPort,
                                _ => _.ToPort,
                                _ => _.ShipmentDate,
                                _ => _.Etd,
                                _ => _.Eta,
                                _ => _.Ata,
                                _ => _.FeedervesselName,
                                _ => _.OceanvesselName,
                                _ => _.OceanvesselvoyageNo,
                                _ => _.NoofinvoicewithinshipmentNo,
                                _ => _.Noof20Ftcontainers,
                                _ => _.Noof40Ftcontainers,
                                _ => _.Lclvolume,
                                _ => _.Atd,
                                _ => _.Status,
                                _ => _.IsActive

                                );

                  
                });

        }
    }
}
