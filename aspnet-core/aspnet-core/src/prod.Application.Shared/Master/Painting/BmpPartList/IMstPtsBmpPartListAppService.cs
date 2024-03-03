using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Painting.Dto;
using prod.Master.Painting.BmpPartList.ImportDto;

namespace prod.Master.Painting
{
	public interface IMstPtsBmpPartListAppService : IApplicationService
	{
		Task<PagedResultDto<MstPtsBmpPartListDto>> GetAll(GetMstPtsBmpPartListInput input);

		Task CreateOrEdit(CreateOrEditMstPtsBmpPartListDto input);

		Task Delete(EntityDto input);
        Task<FileDto> GetBmpPartListToExcel(GetMstPtsBmpPartListExcelInput input);

		Task<List<ImportMstPtsBmpPartListDto>> ImportBmpPartListFromExcel(List<ImportMstPtsBmpPartListDto> bmppartlists);

		Task MergeDataBmpPartList(string v_Guid);
	}
}


