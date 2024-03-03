using Abp.Application.Services.Dto;
using prod.Master.Inventory.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace prod.Master.Inventory
{
	public interface IMstGpsMaterialCategoryMappingAppService
	{
		Task<PagedResultDto<MstGpsMaterialCategoryMappingDto>> GetAll(GetMstGpsMaterialCategoryMappingInput input);
		Task<List<MstGpsMaterialCategoryMappingImportDto>> ImportMaterialCategoryMappingFromExcel(byte[] fileBytes, string fileName);

    }
}
