using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Painting.Andon.Dto;

namespace prod.Painting.Andon
{

	public interface IPtsAdoBumperAppService : IApplicationService
	{

		Task<PagedResultDto<PtsAdoBumperDto>> GetAll(GetPtsAdoBumperInput input);


		Task CreateOrEdit(CreateOrEditPtsAdoBumperDto input);

		Task Delete(EntityDto input);

		Task<PagedResultDto<PtsAdoBumperGetDataSmallSubassyDto>> GetBumperDataSmallSubassy();

		Task<FileDto> GetBumperToExcel(GetPtsAdoBumperExportInput input);


    }
	//bumper clr
	public interface IPtsAdoBumperGetdataClrIndicatorAppService : IApplicationService
	{

		Task<PagedResultDto<PtsAdoBumperGetdataClrIndicatorDto>> GetBumperGetdataClrIndicator();
	}


}



