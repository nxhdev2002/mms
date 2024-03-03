using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CPS.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inventory.CPS.InvCpsRcvShipmentHeader.Exproting
{

    public interface IInvCpsRcvShipmentHeadersExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvCpsRcvShipmentHeadersDto> invcpsrcvshipmentheaders);
        FileDto ExportToFileLine(List<InvCpsRcvShipmentLineDto> invcpsrcvshipmentlines);

    }

}


