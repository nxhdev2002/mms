using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prod.LogA.Bp2.Dto;

namespace prod.LogA.Bp2
{
    
    public class LgaBp2BigPartDSMAppService : prodAppServiceBase, ILgaBp2BigPartDSMAppServices
    {
        private readonly IDapperRepository<LgaBp2PxPUpPlan, long> _dapperRepo;

        public LgaBp2BigPartDSMAppService(IDapperRepository<LgaBp2PxPUpPlan, long> dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }

        public async Task<PagedResultDto<LgaBp2BigPartDSMDto>> GetDataBigPartDSM(string prod_line)
        {
            string _sql = "Exec LGA_BP2_BIG_PART_MONITOR_GET_DATA @prod_line";

            var filtered = await _dapperRepo.QueryAsync<LgaBp2BigPartDSMDto>(_sql, new { @prod_line = prod_line });
            var results = from o in filtered
                          select new LgaBp2BigPartDSMDto
                          {
                              Title = o.Title,
                              ProdLine = o.ProdLine,
                              WorkingDate = o.WorkingDate,
                              NoInShift = o.NoInShift,
                              EcarName = o.EcarName,
                              Code = o.Code,
                              ProcessName = o.ProcessName,
                              Sorting = o.Sorting,
                              TotalCycle = o.TotalCycle,
                              SequenceNo = o.SequenceNo,
                              Efficiency = o.Efficiency,
                              DelaySecond = o.DelaySecond,
                              TaktTime = o.TaktTime,
                              IsPaused = o.IsPaused,
                              IsStoped = o.IsStoped,
                              Status = o.Status,
                              IsDelay = o.IsDelay
                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<LgaBp2BigPartDSMDto>(
                totalCount,
                results.ToList()
            );
        }

    }
}
