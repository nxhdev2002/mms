using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Bp2PartList.ImportDto;

namespace prod.Master.LogA
{

	public interface IMstLgaBp2PartListAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgaBp2PartListDto>> GetAll(GetMstLgaBp2PartListInput input);

		Task CreateOrEdit(CreateOrEditMstLgaBp2PartListDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBp2PartListToExcel(GetMstLgaBp2PartListExcelInput input);


        Task<List<ImportMstLgaBp2PartListDto>> ImportBp2PartListFromExcel(List<ImportMstLgaBp2PartListDto> bp2partlists);

		Task MergeDataBp2PartList(string v_Guid);
	}

}


