using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inventory.LotPart.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.LotPart.Exporting
{
    public interface IMstInvLotPartExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstInvLotPartDto> mstinvlotpart);
    }
}
