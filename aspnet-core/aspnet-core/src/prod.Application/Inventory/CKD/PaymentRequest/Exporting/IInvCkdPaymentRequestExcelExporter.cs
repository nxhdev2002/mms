using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.PaymentRequest.Exporting
{
    public interface IInvCkdPaymentRequestExcelExporter : IApplicationService
    {
        FileDto ExportToFilePaymentRequest(List<InvCkdPaymentRequestDto> invckdpaymentrequest);

        FileDto ExportToFileCustomsDeclare(List<InvCkdCustomsDeclarePmDto> invckdcustomsdeclare);
    }
}
