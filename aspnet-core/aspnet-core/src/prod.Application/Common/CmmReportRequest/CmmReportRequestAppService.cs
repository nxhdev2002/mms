using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Common;
using prod.Common.Dto;
using prod.LogA.Bp2.Dto;
using prod.LogA.Bp2;

namespace prod.Master.Common
{
   // [AbpAuthorize(AppPermissions.Pages_Cmm_ReportRequest)]
    public class CmmReportRequestAppService : prodAppServiceBase, ICmmReportRequestAppService
    {

        private readonly IDapperRepository<CmmReportRequest, long> _dapperRepo;

        public CmmReportRequestAppService(IDapperRepository<CmmReportRequest, long> dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }

        public async Task<PagedResultDto<CmmReportRequestDto>> GetDatabyUser(GetCmmReportRequestInput input)
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            var userId = user.Id;

            string _sql = "Exec CMM_REPORT_REQUEST_GET_DATA_BY_USER @ReportList,@ReqStatus,@ReqTime,@UserId";
            var filtered = await _dapperRepo.QueryAsync<CmmReportRequestDto>(_sql, 
                new {
                    @ReportList = input.ReportList,
                    @ReqStatus = input.ReqStatus,
                    @ReqTime = input.ReqTime,
                    @UserId = userId,
                });

            var results = from o in filtered
                          select new CmmReportRequestDto
                          {
                              Id = o.Id,
                              ReportList = o.ReportList,
                              ReportParams = o.ReportParams,
                              ReqStatus = o.ReqStatus,
                              ReqTime = o.ReqTime,
                              IsActive = o.IsActive
                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<CmmReportRequestDto>(
                totalCount,
                results.ToList()
            );
        }

    }
}
