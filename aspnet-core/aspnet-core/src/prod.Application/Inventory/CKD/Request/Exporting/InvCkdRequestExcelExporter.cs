using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.InvCkdRequest.Exporting
{

    public class InvCkdRequestExcelExporter : NpoiExcelExporterBase, IInvCkdRequestExcelExporter
    {
        public InvCkdRequestExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFileRequest(List<InvCkdRequestDto> invckdrequest)
        {
            return CreateExcelPackage(
                "InventoryCkdRequest.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Request");
                    AddHeader(
                                sheet,
                                ("ReqDate"),
                                ("CkdReqNo"),
                                ("IssueDate"),
                                ("Version"),
                                ("Status")
                                );
                    AddObjects(
                         sheet, invckdrequest,
                                _ => _.ReqDate,
                                _ => _.CkdReqNo,
                                _ => _.IssueDate,
                                _ => _.Version,
                                _ => _.Status
                                );

                   
                });
        }

        public FileDto ExportToFileRequestByCall(List<GetInvCkdDeliveryScheGetByReqByCallDto> invckdrequestbycall)
        {
            return CreateExcelPackage(
                "InventoryCkdRequestByCall.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Request By Call");
                    AddHeader(
                                sheet,
                                ("Shop"),
                                ("Status"),
                                ("TimeRequest"),
                                ("ContainerNo"),
                                ("Renban"),
                                ("LotNo"),
                                ("LotFollow"),
                                ("BillofladingNo"),
                                ("InvoiceNo"),
                                ("ListLotNo"),
                                ("ListcaseNo"),
                                ("Source"),
                                ("Transport"),
                                ("CdDate"),
                                ("ReturnType"),
                                ("DevanningDate"),
                                ("DevanningTime"),
                                ("ActualDevanningDate"),
                                ("ActualDevanningTime"),
                                ("ContStatus")
                                );
                    AddObjects(
                         sheet, invckdrequestbycall,
                                _ => _.Shop,
                                _ => _.Status,
                                _ => _.TimeRequest,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.LotNo,
                                _ => _.LotFollow,
                                _ => _.BillofladingNo,
                                _ => _.InvoiceNo,
                                _ => _.ListLotNo,
                                _ => _.ListcaseNo,
                                _ => _.Source,
                                _ => _.Transport,
                                _ => _.CdDate,
                                _ => _.ReturnType,
                                _ => _.DevanningDate,
                                _ => _.DevanningTime,
                                _ => _.ActualDevanningDate,
                                _ => _.ActualDevanningTime,
                                _ => _.ContStatus
                                );

                });
        }

        public FileDto ExportToFileRequestByLot(List<InvCkdRequestContentByLotDto> invckdrequestbylot)
        {
            return CreateExcelPackage(
                "InventoryCkdRequestByLot.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Request By Lot");
                    AddHeader(
                                sheet,
                                ("Dock"),
                                ("TimeRequest"),
                                ("Grade"),
                                ("LotNo"),
                                ("Robbing"),
                                ("Remarks")
                                );
                    AddObjects(
                         sheet, invckdrequestbylot,
                                _ => _.Dock,
                                _ => _.TimeRequest,
                                _ => _.Grade,
                                _ => _.LotNo,
                                _ => _.Robbing,
                                _ => _.Remarks
                                );

                  
                });
        }

        public FileDto ExportToFileRequestByMake(List<GetInvCkdDeliveryScheGetByReqByMakeDto> invckdrequestbymake)
        {
            return CreateExcelPackage(
                "InventoryCkdRequestByMake.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Request By Make");
                    AddHeader(
                                sheet,
                                ("Shop"),
                                ("Status"),
                                ("TimeRequest"),
                                ("PcdTimeRequest"),
                                ("ContainerNo"),
                                ("Renban"),
                                ("LotNo"),
                                ("LotFollow"),
                                ("ContainerSize"),
                                ("ShippingcompanyCode"),
                                ("ReturnType"),
                                ("BillofladingNo"),
                                ("InvoiceNo"),
                                ("ListLotNo"),
                                ("ListcaseNo"),
                                ("Source"),
                                ("Transporter"),
                                ("CdDate"),
                                ("DevanningDate"),
                                ("DevanningTime"),
                                ("ActualDevanningDate"),
                                ("ActualDevanningTime"),
                                ("ContStatus")
                                //("PartNo"),
                                //("PartName"),
                                
                                );
                    AddObjects(
                         sheet, invckdrequestbymake,
                                _ => _.Shop,
                                _ => _.Status,
                                _ => _.TimeRequest,
                                _ => _.PcdTimeRequest,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.LotNo,
                                _ => _.LotFollow,
                                _ => _.ContainerSize,
                                _ => _.ShippingcompanyCode,
                                _ => _.ReturnType,
                                _ => _.BillofladingNo,
                                _ => _.InvoiceNo,
                                _ => _.ListLotNo,
                                _ => _.ListcaseNo,
                                _ => _.Source,
                                _ => _.Transporter,
                                _ => _.CdDate,
                                _ => _.DevanningDate,
                                _ => _.DevanningTime,
                                _ => _.ActualDevanningDate,
                                _ => _.ActualDevanningTime,
                                _ => _.ContStatus
                                //_ => _.PartNo,
                                //_ => _.PartName,
                               
                                );

                  
                });
        }

        public FileDto ExportToFileRequestByPxp(List<InvCkdRequestContentByPxPDto> invckdrequestbypxp)
        {
            return CreateExcelPackage(
                "InventoryCkdRequestByPxP.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Request By Pxp");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartName"),
                                ("Source"),
                                ("Model"),
                                ("StockendofReqDate"),
                                ("MinstockAtTmv"),
                                ("Balance"),
                                ("ReqQuantity"),
                                ("TimeRequest"),
                                ("Remarks")
                                );
                    AddObjects(
                         sheet, invckdrequestbypxp,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Source,
                                _ => _.Model,
                                _ => _.StockendofReqDate,
                                _ => _.MinstockAtTmv,
                                _ => _.Balance,
                                _ => _.ReqQuantity,
                                _ => _.TimeRequest,
                                _ => _.Remarks
                                );

                });
        }
    }
}
