using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;
using prod.LogA.Lds.Dto;
 
namespace prod.LogA.Lds
{
    public class LotDirectSupplyAndonAppService : prodAppServiceBase, ILotDirectSupplyAndonAppService
    {
        private readonly IRepository<LgaLdsLotPlan, long> _repo;
        private readonly IDapperRepository<LgaLdsLotPlan, long> _dapperRepo;

        public LotDirectSupplyAndonAppService(IRepository<LgaLdsLotPlan, long> repo,
                                                IDapperRepository<LgaLdsLotPlan, long> dapperRepo)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
        }

        public async Task<PagedResultDto<LotDirectSupplyAndonDto>> GetDataLotDirectSupplyAndon(string prod_line)
        {
            string _sql = "Exec LGA_LDS_LOT_PLAN_GET_DATA @prod_line";

            var filtered = await _dapperRepo.QueryAsync<LotDirectSupplyAndonDto>(_sql, new { @prod_line = prod_line });
            var results = from o in filtered
                          select new LotDirectSupplyAndonDto
                          {
                              Title = o.Title,
                              DelaySecond = o.DelaySecond,
                              ActualTrim = o.ActualTrim,
                              PlanTrim = o.PlanTrim,
                              Shift = o.Shift,
                              SeqLineIn = o.SeqLineIn,
                              Trip = o.Trip,
                              Dolly = o.Dolly,
                              StartDatetime = o.StartDatetime,
                              FinishDatetime = o.FinishDatetime,
                              NextDolly = o.NextDolly,
                              TotalTaktTime = o.TotalTaktTime,
                              TripTatkTime = o.TripTatkTime,
                              TripActualTime = o.TripActualTime,
                              NewTaktStartTime = o.NewTaktStartTime,
                              IsDelayStart = o.IsDelayStart,
                              ScreenStatus = o.ScreenStatus,
                              Status = o.Status,
                              TotalTrip = o.TotalTrip,
                              PlanVolCount = o.PlanVolCount,
                              NTSignalCount = o.NTSignalCount
                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<LotDirectSupplyAndonDto>(
                totalCount,
                results.ToList()
            );
        }

        public async Task<PagedResultDto<LotDirectSupplyMonitoringDto>> GetDataDirectSupplyMonitor(string prod_line)
        {            
            string _sql = "Exec LGA_LDS_LOT_PLAN_MONITOR_GET_DATA @prod_line";

            var filtered = await _dapperRepo.QueryAsync<LotDirectSupplyMonitoringDto>(_sql, new { @prod_line = prod_line });
            var results = from o in filtered
                          select new LotDirectSupplyMonitoringDto
                          {
                              Title = o.Title,
                              TotalCycle = o.TotalCycle,
                              SequenceNo = o.SequenceNo,
                              Efficiency = o.Efficiency,
                              DelaySecond = o.DelaySecond,
                              SeqLineIn = o.SeqLineIn,
                              IsDelay = o.IsDelay,
                              TaktTime = o.TaktTime,
                              IsPaused = o.IsPaused,
                              IsStoped = o.IsStoped,
                              Status = o.Status
                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<LotDirectSupplyMonitoringDto>(
                totalCount,
                results.ToList()
            );
        }

    }
}
