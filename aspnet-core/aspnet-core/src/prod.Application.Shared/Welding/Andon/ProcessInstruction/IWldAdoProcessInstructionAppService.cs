using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Welding.Andon.Dto;

namespace prod.Welding.Andon
{

	public interface IWldAdoProcessInstructionAppService : IApplicationService
	{

		Task<List<GetProcessInstructionDataOutput>> GetProcessInstructionData(string pWLine, string pProcess);


	}

}


