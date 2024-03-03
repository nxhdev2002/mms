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
using prod.Painting.Andon.Dto;
using prod.Painting.Andon.Exporting;

namespace prod.Painting.Andon
{
    //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_LineEfficiency)]
    public class PtsAdoLineRealTimeControlAppService : prodAppServiceBase, IPtsAdoLineRealTimeControlAppService
    {
        private readonly IDapperRepository<PtsAdoLineEfficiency, long> _dapperRepo;
        private readonly IRepository<PtsAdoLineEfficiency, long> _repo;

        public PtsAdoLineRealTimeControlAppService(IRepository<PtsAdoLineEfficiency, long> repo,
                                         IDapperRepository<PtsAdoLineEfficiency, long> dapperRepo
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
        }

        public async Task<PagedResultDto<GetDetailsOutput>> GetDetails()
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 300;
            string _sql = "Exec PTS_ADO_LINE_REAL_TIME_CONTROL_GETS_DETAILS ";
            var filtered = await _dapperRepo.QueryAsync<GetDetailsOutput>(_sql, new { });

            var results = from d in filtered
                          select new GetDetailsOutput
                          {
                              Line = d.Line,
                              Shift = d.Shift,
                              WorkingDate = d.WorkingDate,
                              VolTarget = d.VolTarget,
                              VolActual = d.VolActual,
                              VolBalance = d.VolBalance,
                              StopTime = d.StopTime,
                              Efficiency = d.Efficiency,
                              TaktTime = d.TaktTime,
                              Overtime = d.Overtime,
                              NonProdAct = d.NonProdAct,
                              ShiftVolPlan = d.ShiftVolPlan,
                              OffLine1 = d.OffLine1,
                              OffLine2 = d.OffLine2,
                              OffLine3 = d.OffLine3,
                              OffLine4 = d.OffLine4,
                              OffLine5 = d.OffLine5,
                              OffLine6 = d.OffLine6,
                              OffLine7 = d.OffLine7,
                              OffLine8 = d.OffLine8
                          };


            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<GetDetailsOutput>(
                totalCount,
                results.ToList()
            );
        }

    }
}
