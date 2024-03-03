using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;
using prod.Master.LogW.RenbanModule.ImportDto;

namespace prod.Master.LogW
{

	public interface IMstLgwRenbanModuleAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgwRenbanModuleDto>> GetAll(GetMstLgwRenbanModuleInput input);

		Task CreateOrEdit(CreateOrEditMstLgwRenbanModuleDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetRenbanModuleToExcel(MstLgwRenbanModuleExportInput input);

        Task<List<ImportMstLgwRenbanModuleDto>> ImportMstLgwRenbanModuleFromExcel(List<ImportMstLgwRenbanModuleDto> MstLgwRenbanModules);
		Task MergeDataMstLgwRenbanModule(string v_Guid);
	}

}


