using Abp.Dapper.Repositories; 
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks; 

namespace prod.LogW.Lup
{
   
    public class LgwLupLotUnPackingAndonAppService : prodAppServiceBase, ILgwLupLotUnPackingAndonAppService
    {
        private readonly IDapperRepository<LgwLupLotUpPlan, long> _dapperRepo; 

        public LgwLupLotUnPackingAndonAppService(IDapperRepository<LgwLupLotUpPlan, long> dapperRepo)
        { 
            _dapperRepo = dapperRepo; 
        }

        public async Task<List<GetLotUnPackingAndonOutput>> LgwLupLotUnPackingAndonGetData(string p_Line)
        {

            string _sql = "Exec LGW_LUP_LOT_UP_PLAN_GET_DATA @prod_line";

            var filtered = await _dapperRepo.QueryAsync<GetLotUnPackingAndonOutput>(_sql, new
            {
                prod_line = p_Line
            });

            return filtered.ToList();
        }


    }
}
