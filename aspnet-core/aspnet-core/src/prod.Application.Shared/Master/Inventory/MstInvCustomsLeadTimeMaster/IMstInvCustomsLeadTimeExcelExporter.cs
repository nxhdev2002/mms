using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Inventory
{
    public interface IMstInvCustomsLeadTimeExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstInvCustomsLeadTimeDto> mstinvCustomsLeadTime);
    }
}
