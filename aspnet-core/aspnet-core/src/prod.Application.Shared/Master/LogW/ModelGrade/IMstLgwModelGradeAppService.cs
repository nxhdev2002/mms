using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;
using prod.Master.LogW.ModelGrade.ImportDto;

namespace prod.Master.LogW
{

	public interface IMstLgwModelGradeAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgwModelGradeDto>> GetAll(GetMstLgwModelGradeInput input);

		Task CreateOrEdit(CreateOrEditMstLgwModelGradeDto input);

		Task Delete(EntityDto input);

		Task<List<ImportMstlgwModelGradeDto>> ImportMstLgwModelgradeFromExcel(List<ImportMstlgwModelGradeDto> MstLgwModelGrades);

		Task MergeDataMstLgwModel(string v_Guid);

	}

}

