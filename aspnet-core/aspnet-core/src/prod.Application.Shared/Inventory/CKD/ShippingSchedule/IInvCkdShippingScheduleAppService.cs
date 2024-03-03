using Abp.Application.Services;
using prod.Inventory.CKD.ShippingSchedule.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.ShippingSchedule
{
    public interface IInvCkdShippingScheduleAppService : IApplicationService
    {
        Task<List<InvShippingScheduleImportDto>> ImportInvShippingScheduleFromExcel(byte[] fileBytes, string fileName);
    }
}
