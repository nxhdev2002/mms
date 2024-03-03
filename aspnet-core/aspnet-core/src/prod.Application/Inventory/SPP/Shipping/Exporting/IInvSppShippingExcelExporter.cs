using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.SPP.Shipping.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.SPP.Shipping.Exporting
{
    public interface IInvSppShippingExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvSppShippingDto> InvSppShipping);
    }
}
