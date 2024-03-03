using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inventory.Dto;
using System.Collections.Generic;

namespace prod.Master.Inventory
{
	public interface IMstInvDemDetDaysExcelExporter : IApplicationService
	{
		FileDto ExportToFile(List<MstInvDemDetDaysDto> mstinvdemdetdays);
        FileDto ExportToFileErr(List<MstInvDemDetDaysImportDto> mstinvdemdetdays_err);
    }
}
