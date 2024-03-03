using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Painting.Dto;
using prod.Master.Painting.BmpPartType.Dto;

namespace prod.Master.Painting
{

	public interface IMstPtsBmpPartTypeAppService : IApplicationService
	{

		Task<PagedResultDto<MstPtsBmpPartTypeDto>> GetAll(GetMstPtsBmpPartTypeInput input);

		Task CreateOrEdit(CreateOrEditMstPtsBmpPartTypeDto input);

		Task Delete(EntityDto input);

	}

}
