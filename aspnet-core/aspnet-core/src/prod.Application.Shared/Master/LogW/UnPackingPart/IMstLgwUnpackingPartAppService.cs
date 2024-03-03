using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;
using prod.Master.LogW.UnPackingPart.ImportDto;

namespace prod.Master.LogW
{

	public interface IMstLgwUnpackingPartAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgwUnpackingPartDto>> GetAll(GetMstLgwUnpackingPartInput input);

		Task CreateOrEdit(CreateOrEditMstLgwUnpackingPartDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetUnpackingPartToExcel(MstLgwUnpackingPartExportInput input);


        Task<List<ImportMstLgwUnpackingPartDto>> ImportUnpackingPartFromExcel(List<ImportMstLgwUnpackingPartDto> unpackingParts);

		Task MergeDataUnpackingPart(string v_Guid);



	}

}


