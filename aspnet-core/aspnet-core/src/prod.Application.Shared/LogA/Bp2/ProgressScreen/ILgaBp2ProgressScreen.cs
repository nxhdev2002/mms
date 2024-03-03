using Abp.Application.Services; 
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace prod.LogA.Bp2
{

	public interface ILgaBp2ProgressScreen : IApplicationService
	{

		Task<List<LgaBp2ProgressScreenConfigOutputDto>> GetScreenConfig(string prod_line);

		Task<List<LgaBp2ProgressScreenConfigOutputDto>> GetScreenData(string prod_line);

		Task<List<LgaBp2ProgressMonitorScreenDto>> GetMonitorScreenData(string prod_line);


	}

}


