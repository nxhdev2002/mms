using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Painting.Andon;
using prod.Welding.Andon.NextEdIn;
using prod.Welding.Andon.NextEdIn.Dto;

namespace prod.Welding.Andon.NExtEdIIn
{
    public class PtsAdoWeldingAppService : IPtsAdoWeldingAppService
    {
        private readonly IDapperRepository<PtsAdoTotalDelay, long> _dapperRepo;
        private readonly IRepository<PtsAdoTotalDelay, long> _repo;

        public PtsAdoWeldingAppService(IRepository<PtsAdoTotalDelay, long> repo,
                                         IDapperRepository<PtsAdoTotalDelay, long> dapperRepo
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
        }
        public async Task<PagedResultDto<GetNextEDInOutput>> GetNextEDIn()
        {
            string _sql = "Exec PTS_ADO_WELDING_GET_NEXT_ID_IN";

            var filtered = await _dapperRepo.QueryAsync<GetNextEDInOutput>(_sql, new { });
            var Filtered1 = filtered.OrderBy(s => s.BodyNo).Take(3);
            var results = from d in Filtered1
                          select new GetNextEDInOutput
                          {
                              ScanLocation = d.ScanLocation,
                              BodyNo = d.BodyNo,
                             
                          };

            var totalCount = Filtered1.ToList().Count;

            return new PagedResultDto<GetNextEDInOutput>(
                totalCount,
                results.ToList()
            );
        }
    }
}