using Abp.Dapper.Repositories;  
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;
using prod.Assy.Andon;
using prod.Master.Common;
using Abp.Domain.Repositories;

namespace prod.Assy.Sps
{
    public class AsyAdoAssemblyScreenAppService : prodAppServiceBase, IAsyAdoAssemblyScreenAppService
    {
        private readonly IDapperRepository<AsyAdoAssemblyData, long> _dapperRepo; 

        public AsyAdoAssemblyScreenAppService(IDapperRepository<AsyAdoAssemblyData, long> dapperRepo)
        {
             _dapperRepo = dapperRepo; 
        } 

        public async Task<List<AsyAdoAssemblyScreenDataOutputDto>> GetScreenData(string prod_line)
        {
            string _sql = "Exec ASY_ADO_SPS_ASSEMBLY_SCREEN_GET_DATA @prod_line"; 

            var _sqldata = await _dapperRepo.QueryAsync<AsyAdoAssemblyScreenDataOutputDto>(_sql, new
            {
                prod_line = prod_line
            });

            return _sqldata.ToList();
        }

        public async Task<List<AsyAdoAssemblyScreenConfigOutputDto>> GetScreenConfig(string screen_code)
        {
            string _sql = "Exec ASY_ADO_SPS_ASSEMBLY_SCREEN_GET_SCREEN_CONFIG @screen_code";

            var _sqldata = await _dapperRepo.QueryAsync<AsyAdoAssemblyScreenConfigOutputDto>(_sql, new
            {
                screen_code = screen_code
            });

            return _sqldata.ToList();
        }

       
    }
}
