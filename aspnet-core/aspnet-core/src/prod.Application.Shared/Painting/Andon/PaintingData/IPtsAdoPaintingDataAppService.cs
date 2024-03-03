using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.Painting.Andon.Dto;

namespace prod.Painting.Andon
{

	public interface IPtsAdoPaintingDataAppService : IApplicationService
	{

		Task<PagedResultDto<PtsAdoPaintingDataDto>> GetAll(GetPtsAdoPaintingDataInput input);
        Task<List<PtsAdoBumperGetDataBumperInDto>> GetBumperGetdataBumperIn(string prod_line, string process);
        Task<List<PtsBmpPartTypeDto>> GetPartTypeBumperIn(string prod_line, string process);

        Task<List<PtsAdoBumperGetDataBumperInDto>> ConfirmPartBumperIn(string progress_id);

        Task<List<PtsAdoBumperGetDataBumperInDto>> UpdateStatusBumperIn(string progress_id);


    }

}