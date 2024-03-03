using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Cmm.Dto;

namespace prod.Master.Cmm
{

	public interface IMstCmmModelAppService : IApplicationService
	{

		Task<PagedResultDto<MstCmmModelDto>> GetAllModel(GetMstCmmModelInput input);

		Task<PagedResultDto<MstCmmLotCodeGradeDto>> GetAllLotCodeGrade(GetMstCmmLotCodeGradeInput input);


        Task CreateOrEdit(CreateOrEditMstCmmModelDto input);

		Task Delete(EntityDto input);

	}

}


