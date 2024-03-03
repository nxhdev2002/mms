using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD.Exporting
{

    public interface IInvCkdStockIssuingExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdStockIssuingDto> invckdstockissuing);
        FileDto ExportByMaterialToFile(List<InvCkdStockPartByMaterialDto> invckdstockissuingbymaterial);
        FileDto ExportStockIssuingViewToFile(List<InvCkdStockIssuingTranslocDto> stockissuing);
        FileDto ExportStockIssuingValidate(List<InvCkdStockIssuingValidateDto> checkstockissuing);
        FileDto ExportToHistoricalFile(List<string> data);
    }

}


