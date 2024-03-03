using Abp.Application.Services; 
using System.Collections.Generic;
using System.Threading.Tasks;
 
namespace prod.Assy.Sps
{
	public interface IAsyAdoAssemblyScreenAppService : IApplicationService
	{
		 
		Task<List<AsyAdoAssemblyScreenDataOutputDto>> GetScreenData(string prod_line);
		Task<List<AsyAdoAssemblyScreenConfigOutputDto>> GetScreenConfig(string screen_code);
		 

    }

}


