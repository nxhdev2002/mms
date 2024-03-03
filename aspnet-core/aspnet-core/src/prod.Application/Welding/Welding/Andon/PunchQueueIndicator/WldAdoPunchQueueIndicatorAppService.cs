using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;
using prod.Welding.Andon.WldPunchQueueIndicator;
using prod.Welding.Andon.PunchQueueIndicator.Dto;
using prod.Painting.Andon;
using Abp.Authorization;
using prod.Authorization;
using prod.LogA.Bp2;
using System.Collections.Generic;

namespace prod.Welding.Andon.PunchQueueIndicator
{
    public class WldAdoPunchQueueIndicatorAppService : IWldAdoPunchQueueIndicatorAppService
    {
        private readonly IDapperRepository<PtsAdoTotalDelay, long> _dapperRepo;
        private readonly IRepository<PtsAdoTotalDelay, long> _repo;

        public WldAdoPunchQueueIndicatorAppService(IRepository<PtsAdoTotalDelay, long> repo,
                                         IDapperRepository<PtsAdoTotalDelay, long> dapperRepo
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
        }
 
        //  [AbpAuthorize(AppPermissions.Pages_Welding_Andon_WeldingPlan_Edit)]
        public async Task<PagedResultDto<WldAdoPunchQueueIndicatorDto>> getWldAdoPunchQueueIndicator()
        {
            string _sql = "Exec WLD_ADO_PUNCH_QUEUE_INDICATOR_GET_DATA";

            var filtered = await _dapperRepo.QueryAsync<WldAdoPunchQueueIndicatorDto>(_sql, new { });

           var Filtered1= filtered.OrderBy(s => s.Filler).Take(3);

            var results = from d in Filtered1
                          select new WldAdoPunchQueueIndicatorDto
                          {   Filler = d.Filler,
                              Seq = d.Seq,
                              ProcessCd = d.ProcessCd,
                              BodyNo = d.BodyNo,
                              Mode = d.Mode,
                              Line = d.Line,
                              LotNo = d.LotNo,
                              Color = d.Color,
                              ProcessGroup = d.ProcessGroup,
                              ScanTime = d.ScanTime,
                              PunchFlag = d.PunchFlag,
                              PunchIndicator = d.PunchIndicator,
                              WTalkTime = d.WTalkTime,
                              WeldSignal = d.WeldSignal
                          };

            var totalCount = Filtered1.ToList().Count;

            return new PagedResultDto<WldAdoPunchQueueIndicatorDto>(
                totalCount,
                results.ToList()
            );
        }
     


        //  [AbpAuthorize(AppPermissions.Pages_Welding_Andon_PunchQueueIndicator)]
        public async Task<List<WldAdoPunchQueueIndicatorV2Dto>> getWldAdoPunchQueueIndicatorV2()
        {
            string _sql = "Exec WLD_ADO_PUNCH_QUEUE_INDICATOR_GET_DATA";

            var _sqldata = await _dapperRepo.QueryAsync<WldAdoPunchQueueIndicatorV2Dto>(_sql, new
            {
                
            });

            return _sqldata.ToList();
        }
    }
}