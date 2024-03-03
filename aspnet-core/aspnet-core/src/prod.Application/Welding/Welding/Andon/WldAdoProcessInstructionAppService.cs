using Abp.Dapper.Repositories; 
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks; 
using prod.Welding.Andon.Dto; 

namespace prod.Welding.Andon
{
   
    public class WldAdoProcessInstructionAppService : prodAppServiceBase, IWldAdoProcessInstructionAppService
    {
        private readonly IDapperRepository<WldAdoWeldingProgress, long> _dapperRepo; 

        public WldAdoProcessInstructionAppService(IDapperRepository<WldAdoWeldingProgress, long> dapperRepo)
        { 
            _dapperRepo = dapperRepo; 
        }

        public async Task<List<GetProcessInstructionDataOutput>> GetProcessInstructionData(string p_WLine, string p_Process)
        {

            string _sql = "Exec WLD_ADO_PROGRESS_GET_PROCESS_INSTRUCTION_DATA @pWLine, @pProcess";

            var filtered = await _dapperRepo.QueryAsync<GetProcessInstructionDataOutput>(_sql, new
            {
                pWLine = p_WLine,
                pProcess = p_Process
            });

            return filtered.ToList();
        }
 
    }
}
