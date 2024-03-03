using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Common.Dto;

namespace prod.Master.Common
{

	public interface IMstCmmLookupAppService : IApplicationService
	{

		Task<PagedResultDto<MstCmmLookupDto>> GetAll(GetMstCmmLookupInput input);
		

        Task CreateOrEdit(CreateOrEditMstCmmLookupDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLookupToExcel(MstCmmLookupExportInput input);
		Task<List<long>> GetALLMstCmmLookupById();
		Task<List<MstCmmLookupDto>> GetsByDomainCode(string DomainCode);  
        Task SaveAll(List<CreateOrEditMstCmmLookupDto> listData);

    }

}


