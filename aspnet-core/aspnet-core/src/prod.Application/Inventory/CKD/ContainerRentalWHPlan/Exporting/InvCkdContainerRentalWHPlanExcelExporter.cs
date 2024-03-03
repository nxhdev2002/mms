using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdContainerRentalWHPlanExcelExporter : NpoiExcelExporterBase, IInvCkdContainerRentalWHPlanExcelExporter
    {
        public InvCkdContainerRentalWHPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdContainerRentalWHPlanDto> containerrentalwhplan)
        {
            return CreateExcelPackage(
                "InventoryCKDContainerRentalWHPlan.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerRentalWHPlan");
                    AddHeader(
                                sheet,
                                ("ContainerNo"),
								("Code"),
								("Renban"),
                                ("RequestDate"),
                                ("RequestTime"),
                                ("InvoiceNo"),
                                ("BillofladingNo"),
                                ("SupplierNo"),
                                ("SealNo"),
                                ("ListcaseNo"),
                                ("ListLotNo"),
                                ("CdDate"),
                                ("DevanningDate"),
                                ("DevanningTime"),
                                ("ActualDevanningDate"),
                                ("GateInPlanTime"),
                                ("GateInActualDateTime"),
                                ("Transport"),
                                ("PlateId"),
                                ("Status"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet,containerrentalwhplan,
                                _ => _.ContainerNo,
								_ => _.WHCode,
								_ => _.Renban,
                                _ => _.RequestDate,
                                _ => _.RequestTime,
                                _ => _.InvoiceNo,
                                _ => _.BillofladingNo,
                                _ => _.SupplierNo,
                                _ => _.SealNo,
                                _ => _.ListcaseNo,
                                _ => _.ListLotNo,
                                _ => _.CdDate,
                                _ => _.DevanningDate,
                                _ => _.DevanningTime,
                                _ => _.ActualDevanningDate,
                                _ => _.GateInPlanTime,
                                _ => _.GateInActualDateTime,
                                _ => _.Transport,
                                _ => _.PlateId,
                                _ => _.Status,
                                _ => _.IsActive
                                );

                  
                });

        }


        public FileDto ExportToFileErr(List<InvCkdContainerRentalWHPlErrDto> invckdcontainerrentalwhplan_err)
        {
            return CreateExcelPackage(
                "InventoryCKDContainerRentalWHPlanErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerRentalWHPlan");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("DevanningDate"),
                                ("DevanningTime"),
                                ("ContainerNo"),
                                ("Renban"),
                                ("SealNo"),
                                ("Forwade"),
                                ("ErrorDescription")
                          
                               );
                    AddObjects(
                         sheet, invckdcontainerrentalwhplan_err,
                                _ => _.ROW_NO,
                                _ => _.DevanningDate,
                                _ => _.DevanningTime,
                                _ => _.ContainerNo,
                                 _ => _.Renban,
                                _ => _.SealNo,
                                _ => _.Transport,
                                _ => _.ErrorDescription
             
                                );

                 
                });

        }
    }
}
