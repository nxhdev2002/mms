using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdBillExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdBillDto> invckdbill);
        FileDto ExportToHistoricalFile(List<string> data);

    }

}


