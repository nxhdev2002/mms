using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.DRM.StockPartExcel.Dto;
using System.Collections.Generic;

namespace prod.Inventory.DRM.StockPartExcel.Exporting
{
    public interface IInvDrmStockPartExcelExcelExporter : IApplicationService
    {
        FileDto ExportToFileErr(List<InvDrmStockPartExcelImportDto> listimporterr);

    }
}
