using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.Frame.Andon.Dto;

namespace prod.Frame.Andon
{

	public interface IFrmAdoFramePlanA1AppService : IApplicationService
	{

		Task<PagedResultDto<FrmAdoFramePlanA1Dto>> GetAll(GetFrmAdoFramePlanA1Input input);

		Task CreateOrEdit(CreateOrEditFrmAdoFramePlanA1Dto input);

		Task Delete(EntityDto input);

		Task<List<ImportFrmAdoFramePlanA1Dto>> ImportFramePlanA1_V2(List<ImportFrmAdoFramePlanA1Dto> framePlanA1s);

		Task MergeDataFramePlanA1(string v_Guid);

		Task<PagedResultDto<FrmAdoFramePlanA1Dto>> GetMessageErrorFramePlanA1Import(string v_Guid);

		Task<FileDto> GetListErrFramePlanA1ToExcel(string v_Guid);

		Task<FileDto> GetFramePlanA1ToExcel(GetFrmAdoFramePlanA1ExportInput input);



    }

}

