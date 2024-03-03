using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using System.Collections.Generic;

namespace prod.Inventory.CKD.Exporting
{

	public interface IInvCkdStockPartExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<InvCkdStockPartDto> invckdstockpart);

        FileDto ExportToFileCheckStock(List<InvCkdStockReceivingDto> checkstockpart);

        FileDto ExportByMaterialToFile(List<InvCkdStockPartByMaterialDto> invckdstockpartbymaterial);
    }

}


