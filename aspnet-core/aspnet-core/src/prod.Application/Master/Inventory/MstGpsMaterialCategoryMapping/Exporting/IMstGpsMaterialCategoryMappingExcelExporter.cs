using prod.Dto;
using prod.Master.Inventory.Dto;
using System.Collections.Generic;

namespace prod.Master.Inventory.Exporting
{
	public interface IMstGpsMaterialCategoryMappingExcelExporter
	{
		FileDto ExportToFile(List<MstGpsMaterialCategoryMappingDto> mstinvgpsmaterialcategorymapping);
        FileDto ExportToHistoricalFile(List<string> data);

    }
}
