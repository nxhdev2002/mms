using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.PhysicalStockIssuing.Dto;
using prod.Inventory.CKD.ReceivingPhysicalStock.Dto;
using System.Collections.Generic;

namespace prod.Inventory.CKD.Exporting
{
    public interface IInvCkdStockReceivingExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdStockReceivingDto> InvCkdStockReceiving);

        FileDto ExportByMaterialToFile(List<InvCkdStockPartByMaterialDto> invckdstockreceivingbymaterial);

        FileDto ExportValidateToFile(List<InvCkdStockReceivingValidateDto> InvCkdStockReceivingValidate);

        FileDto GetReceivingPhysicalStockToExcel(List<InvCkdReceivingPhysicalStockDto> InvCkdReceivingPhysicalStock);

        FileDto ExportToFileDetailsData(List<InvCkdReceivingPhysStockDetailsDataDto> detailsdata);

    }

}


