using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.GPS.Dto;

namespace prod.Inventory.GPS.Exporting
{

    public interface IInvGpsStockRundownTransactionExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvGpsStockRundownTransactionDto> invgpsstockrundowntransaction);

    }

}


