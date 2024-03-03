using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using prod.Inventory.CKD.Dto;
using NPOI.POIFS.Properties;
using prod.MultiTenancy.Accounting.Dto;
using System.Reflection;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdFrameEngineExcelExporter : NpoiExcelExporterBase, IInvCkdFrameEngineExcelExporter
    {
        public InvCkdFrameEngineExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdFrameEngineDto> FrameEngine)
        {
            return CreateExcelPackage(
                "InventoryCKDFrameEngine.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FrameEngine");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("SupplierNo"),
                                ("BillofladingNo"),
                                ("InvoiceNo"),
                                ("LotNo"),
                                ("LotCode"),
                                ("InvoiceDate"),
                                ("FrameNo"),
                                ("EngineNo"),
                                ("Case/ModuleNo")
                               );
                    AddObjects(
                         sheet, FrameEngine,
                                _ => _.PartNo,
                                _ => _.SupplierNo,
                                _ => _.BillofladingNo,
                                _ => _.InvoiceNo,
                                _ => _.LotNo,
                                _ => _.LotCode,
                                _ => _.InvoiceDate,
                                _ => _.FrameNo,
                                _ => _.EngineNo,
                                _ => _.Module


                                );

                });

        }
    }
}
