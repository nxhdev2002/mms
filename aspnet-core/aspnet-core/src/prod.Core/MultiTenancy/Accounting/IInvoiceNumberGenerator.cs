using System.Threading.Tasks;
using Abp.Dependency;

namespace prod.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}