using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CPS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.InvCkdRequest.Exporting
{
    public interface IInvCkdRequestExcelExporter : IApplicationService
    {
        FileDto ExportToFileRequest(List<InvCkdRequestDto> invckdrequest);
        FileDto ExportToFileRequestByLot(List<InvCkdRequestContentByLotDto> invckdrequestbylot);
        FileDto ExportToFileRequestByPxp(List<InvCkdRequestContentByPxPDto> invckdrequestbypxp);
        FileDto ExportToFileRequestByMake(List<GetInvCkdDeliveryScheGetByReqByMakeDto> invckdrequestbymake);
        FileDto ExportToFileRequestByCall(List<GetInvCkdDeliveryScheGetByReqByCallDto> invckdrequestbycall);


    }
}
