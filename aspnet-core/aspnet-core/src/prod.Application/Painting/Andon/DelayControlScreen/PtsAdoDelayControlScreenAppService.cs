using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Common;
using prod.Master.Common.Dto;
using prod.Painting.Andon.Dto;
using prod.Painting.Andon.Exporting;

namespace prod.Painting.Andon
{
    //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_TotalDelay)]
    public class PtsAdoDelayControlScreenAppService : prodAppServiceBase, IPtsAdoDelayControlScreenAppService
    {
        private readonly IDapperRepository<PtsAdoTotalDelay, long> _dapperRepo;
        private readonly IRepository<PtsAdoTotalDelay, long> _repo;
        private readonly IRepository<MstCmmLookup, long> _repoLookup;



        public PtsAdoDelayControlScreenAppService(IRepository<PtsAdoTotalDelay, long> repo,
                                         IDapperRepository<PtsAdoTotalDelay, long> dapperRepo,
                                         IRepository<MstCmmLookup, long> repoLookup
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _repoLookup = repoLookup;
        }

        public async Task<PagedResultDto<GetDataOutput>> GetData()
        {

            string _sql = "Exec PTS_ADO_TOTAL_DELAY_GETDATA ";

            var filtered = await _dapperRepo.QueryAsync<GetDataOutput>(_sql, new { });

            var results = from d in filtered
                          select new GetDataOutput
                          {
                              RowNumber = d.RowNumber,
                              Id= d.Id,
                              BodyNo = d.BodyNo,
                              Color = d.Color,
                              LeadtimePlus = d.LeadtimePlus,
                              TotalDelay = d.TotalDelay,
                              TotalDelayAct = d.TotalDelayAct,
                              Etd = d.Etd,
                              AInPlanDate = d.AInPlanDate,
                              Mode = d.Mode,
                              Location = d.Location,
                              DifRepairIn = d.DifRepairIn,
                              DiffStartRepair = d.DiffStartRepair,
                              DiffRecoat = d.DiffRecoat,
                              DiffNow = d.DiffNow,
                              DiffETD = d.DiffETD,
                              RemainingTime = d.RemainingTime,
                              RepairIn = d.RepairIn,
                              StartRepair = d.StartRepair,
                              RecoatIn = d.RecoatIn,
                              Leadtime = d.Leadtime,
                              LotNo = d.LotNo,
                              IsActive = d.IsActive
                          };


            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<GetDataOutput>(
                totalCount,
                results.ToList()
            );
        }

        public async Task<PagedResultDto<MstCmmLookupDto>> GetLookupDelayTaget(string ItemCode)
        {

            var filtered = _repoLookup.GetAll()
                .Where(e => e.DomainCode == "PT_TOTAL_DELAY" && e.ItemCode == ItemCode);
                
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var results = from o in pageAndFiltered
                         select new MstCmmLookupDto
                         {
                             ItemCode = o.ItemCode,
                             ItemValue = o.ItemValue
                         };

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<MstCmmLookupDto>(
                totalCount,
                results.ToList()
            );
        }
    }
}
