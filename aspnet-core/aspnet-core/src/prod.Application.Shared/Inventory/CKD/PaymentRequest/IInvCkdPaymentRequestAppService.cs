using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CPS.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public interface IInvCkdPaymentRequestAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdCustomsDeclarePmDto>> GetCustomsDeclareByPayment(GetCustomsDeclarePmExportInput input);

        Task<PagedResultDto<InvCkdPaymentRequestDto>> GetPaymentRequestSearch(GetInvCkdPaymentRequestInput input);
    }
}
