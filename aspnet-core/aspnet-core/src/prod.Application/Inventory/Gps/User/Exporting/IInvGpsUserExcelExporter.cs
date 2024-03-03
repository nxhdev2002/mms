using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.Gps.User.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.User.Exporting
{
    public interface IInvGpsUserExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvGpsUserDto> invgpsuser);
    }
}
