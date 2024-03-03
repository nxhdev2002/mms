using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Storage;

namespace vovina.Inventory.CKD.Exporting
{
    public class InvCkdContainerTransitPortPlanExcelExporter : NpoiExcelExporterBase, IInvCkdContainerTransitPortPlanExcelExporter
    {
        public InvCkdContainerTransitPortPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdContainerTransitPortPlanDto> containertransitportplan)
        {
            return CreateExcelPackage(
                "InventoryCKDContainerTransitPortPlan.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerTransitPortPlan");
                    AddHeader(
                                sheet,
                                ("ContainerNo"),
                                ("Renban"),
                                ("RequestDate"),
                                ("RequestTime"),
                                ("ReceiveDate"),
                                ("InvoiceNo"),                    
                                ("BillOfLadingNo"),
                                ("Customs (VP)"),
                                ("Customs (HP&Trucking)"),
                                ("SupplierNo"),
                                ("SealNo"),
                                ("ListCaseNo"),
                                ("ListLotNo"),
                                ("CdDate"),
                                ("Transport"),
                                ("Status"),
                                ("PortCode"),
                                ("PortName"),
                                ("Remarks"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, containertransitportplan,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.RequestDate,
                                _ => _.RequestTime,
                                _ => _.ReceiveDate,
                                _ => _.InvoiceNo,
                                _ => _.BillOfLadingNo,
                                _ => _.Custums1,
                                _ => _.Custums2,
                                _ => _.SupplierNo,
                                _ => _.SealNo,
                                _ => _.ListCaseNo,
                                _ => _.ListLotNo,
                                _ => _.CdDate,
                                _ => _.Transport,
                                _ => _.Status,
                                _ => _.PortCode,
                                _ => _.PortName,
                                _ => _.Remarks,
                                _ => _.IsActive

                                );

                 
                });

        }
        public FileDto ExportToFileErr(List<InvCkdContainerTransitPortPlanDto> invckdcontainertransitportplan_err)
        {
            return CreateExcelPackage(
                "InvCkdContainerTransitPortPlannErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvCkdContainerTransitPortPlan");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("ContainerNo"),
                                ("Renban"),
                                ("RequestDate"),
                                ("RequestTime"),
                                ("SealNo"),
                                ("Transport"),
                                ("Remarks"),
                                ("ErrorDescription")

                               );
                    AddObjects(
                         sheet, invckdcontainertransitportplan_err,
                                _ => _.ROW_NO,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.RequestDate,
                                _ => _.RequestTime,
                                _ => _.SealNo,
                                _ => _.Transport,
                                _ => _.Remarks,
                                _ => _.ErrorDescription
                                
                                );

                   
                });

        }
    }
}

