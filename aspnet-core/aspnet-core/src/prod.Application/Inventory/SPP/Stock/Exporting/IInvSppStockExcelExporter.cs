using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.SPP.Stock.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.SPP.Stock.Exporting
{
    public interface IInvSppStockExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvSppStockDto> InvSppStock);
    }
}
