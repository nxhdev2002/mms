
using Abp.Authorization;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prod.Authorization; 

namespace prod.LogA.Bp2
{ 

    public class LgaBp2ProgressScreen : prodAppServiceBase, ILgaBp2ProgressScreen
    {
        private readonly IDapperRepository<LgaBp2Progress, long> _dapperRepo; 

        public LgaBp2ProgressScreen(IDapperRepository<LgaBp2Progress, long> dapperRepo)
        {
            _dapperRepo = dapperRepo; 
        }
        

        public async Task<List<LgaBp2ProgressScreenConfigOutputDto>> GetScreenData(string prod_line)
        {
            string _sql = "Exec LGA_BP2_BIG_PART_PROGRESS_GET_DATA_NEW  @prod_line";

            var _sqldata = await _dapperRepo.QueryAsync<LgaBp2ProgressScreenConfigOutputDto>(_sql, new
            {
                prod_line = prod_line
            });

            return _sqldata.ToList();
        }

        public async Task<List<LgaBp2ProgressScreenConfigOutputDto>> GetScreenConfig(string prod_line)
        {
            string _sql = "Exec LGA_BP2_BIG_PART_GET_SCREEN_CONFIG @prod_line";

            var _sqldata = await _dapperRepo.QueryAsync<LgaBp2ProgressScreenConfigOutputDto>(_sql, new
            {
                prod_line = prod_line
            });

            return _sqldata.ToList();
        }



        public async Task<List<LgaBp2ProgressMonitorScreenDto>> GetMonitorScreenData(string prod_line)
        {
            string _sql = "Exec LGA_BP2_BIG_PART_MONITOR_GET_DATA  @prod_line";

            var _sqldata = await _dapperRepo.QueryAsync<LgaBp2ProgressMonitorScreenDto>(_sql, new
            {
                prod_line = prod_line
            });

            return _sqldata.ToList();
        }

    }
}
