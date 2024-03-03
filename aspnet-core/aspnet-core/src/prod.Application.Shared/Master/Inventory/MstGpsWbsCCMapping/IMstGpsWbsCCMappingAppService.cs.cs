using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
	public interface IMstGpsWbsCCMappingAppService : IApplicationService
	{
		Task<PagedResultDto<MstGpsWbsCCMappingDto>> GetAll(GetMstGpsWbsCCMappingInput input);
		Task<List<MstGpsWbsCCMappingImportDto>> ImportGpsWbsCCMappingFromExcel(byte[] fileBytes, string fileName);
	}
}
