using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.DRM.Dto;
using prod.Inventory.DRM.StockPart.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.DRM.StockPart.Exporting
{
    public interface IInvDrmStockPartExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvDrmStockPartDto> invdrmstockpart);

        FileDto ExportToFileErr(List<InvDrmStockPartImportDto> listimporterr);

    }
}
