using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Inventory.CPS.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CPS.InvCpsPrvShipment
{
    public interface IInvCpsRcvShipmentHeadersAppServicecs : IApplicationService
    {
        Task<PagedResultDto<InvCpsRcvShipmentHeadersDto>> GetAllInvCpsRcvShipmentHeaders(GetInvCpsRcvShipmentHeadersInput input);
        Task<PagedResultDto<InvCpsRcvShipmentLineDto>> GetAllInvCpsRcvShipmentHeadersDetail(GetInvCpsRcvShipmentLineInput input);
        Task<ListInvCpsRcvShipmentHeadersDto> GetShipmentHeaderData();
        Task<FileDto> ShipmentHeader_ExportExcel(string p_receipt_num, long? p_invetory_group_id, long? p_vendor_id, DateTime? p_from_date, DateTime? p_to_date, string p_part_no, string p_part_name, string p_po_number);
        Task<FileDto> ShipmentLines_ExportExcel(long? p_id);
    }
}
