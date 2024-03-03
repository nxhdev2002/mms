using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;


namespace prod.Inventory.CKD.PaymentRequest.Exporting
{
    public class InvCkdPaymentRequestExcelExporter : NpoiExcelExporterBase, IInvCkdPaymentRequestExcelExporter
    {
        public InvCkdPaymentRequestExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFileCustomsDeclare(List<InvCkdCustomsDeclarePmDto> invckdcustomsdeclare)
        {
            return CreateExcelPackage(
                "InventoryCKDCustomsDeclare.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CustomsDeclare");
                    AddHeader(
                                sheet,
                                ("InvoiceNo"),
                                ("CustomsDeclareNo"),
                                ("CustomsDate"),
                                ("CustomsPort"),
                                ("Status"),
                                ("OrderType"),
                                ("ImpTax"),
                                ("ImpVat"),
                                ("ActualTax"),
                                ("ActualVat"),
                                ("ExchangeRate")
                               );
                    AddObjects(
                         sheet, invckdcustomsdeclare,
                                _ => _.InvoiceNo,
                                _ => _.CustomsDeclareNo,
                                _ => _.DeclareDate,
                                _ => _.CustomsPort,
                                _ => _.ISPAID,
                                _ => _.OrdertypeCode,
                                _ => _.ImpTax,
                                _ => _.ImpVat,
                                _ => _.ActualTax,
                                _ => _.ActualVat,
                                _ => _.ExchangeRate
                                );

                });
        }

        public FileDto ExportToFilePaymentRequest(List<InvCkdPaymentRequestDto> invckdpaymentrequest)
        {
            return CreateExcelPackage(
                "InventoryCKDPaymentRequest.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PaymentRequest");
                    AddHeader(
                                sheet,
                                ("RequestNo"),
                                ("RequestDate"),
                                ("RequestPerson"),
                                ("FromTxt"),
                                ("RequestDepartment"),
                                ("Status"),
                                ("CustomsPort"),
                                ("Time")
                               );
                    AddObjects(
                         sheet, invckdpaymentrequest,
                                _ => _.Id,
                                _ => _.ReqDate,
                                _ => _.ReqPerson,
                                _ => _.IsFromEcus5,
                                _ => _.ReqDepartment,
                                _ => _.Status,
                                _ => _.CustomsPort,
                                _ => _.Time
                                );

                  
                });
        }
    }
}
