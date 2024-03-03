using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.Gps.FinStock.Dto;
using prod.Inventory.GPS;
using prod.Inventory.GPS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.FinStock.Exporting
{
    public interface IInvGpsFinStockExcelExport : IApplicationService
    {
        FileDto ExportToFile(List<InvGpsFinStockDto> invgpsfinstock);

        FileDto ExportToFileLotErr(List<InvGpsFinStockImportDto> invgpsfinstockerr);
    }
}
