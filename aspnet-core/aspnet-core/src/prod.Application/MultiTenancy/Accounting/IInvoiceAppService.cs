using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using prod.MultiTenancy.Accounting.Dto;

namespace prod.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
