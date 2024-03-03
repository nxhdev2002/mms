using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Inventory
{
	public interface IMstInvDevanningCaseTypeExcelExporter
	{
		FileDto ExportToFile(List<MstInvDevanningCaseTypeDto> mstinvdevanningcasetype);
        FileDto ExportToHistoricalFile(List<string> data);

    }
}
