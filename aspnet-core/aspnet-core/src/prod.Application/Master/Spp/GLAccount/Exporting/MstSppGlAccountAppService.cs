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
using prod.Master.Spp.Dto;
using prod.Master.Spp.Exporting;

namespace prod.Master.Spp
{
    [AbpAuthorize(AppPermissions.Pages_SPP_Master_GLAccount)]
    public class MstSppGlAccountAppService : prodAppServiceBase, IMstSppGlAccountAppService
    {
        private readonly IDapperRepository<MstSppGlAccount, long> _dapperRepo;
        private readonly IRepository<MstSppGlAccount, long> _repo;
        private readonly IMstSppGlAccountExcelExporter _calendarListExcelExporter;

        public MstSppGlAccountAppService(IRepository<MstSppGlAccount, long> repo,
                                         IDapperRepository<MstSppGlAccount, long> dapperRepo,
                                         IMstSppGlAccountExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }


        public async Task<PagedResultDto<MstSppGlAccountDto>> GetAll(GetMstSppGlAccountInput input)
        {
            string _sql = "Exec MASTER_SPP_GL_ACCOUNT_SEARCH @p_GlAccountNo, @p_GlType, @p_StartDate,@p_EndDate";

            IEnumerable<MstSppGlAccountDto> result = await _dapperRepo.QueryAsync<MstSppGlAccountDto>(_sql, new
            {
                p_GlAccountNo = input.GlAccountNo,
                p_GlType = input.GlType,
                p_StartDate = input.StartDate,
                p_EndDate = input.EndDate
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstSppGlAccountDto>(
                totalCount,
                pagedAndFiltered);
        }
        public async Task<FileDto> GetGLAccountToExcel(MstSppGlAccountExportInput input)
        {
            string _sql = "Exec MASTER_SPP_GL_ACCOUNT_SEARCH @p_GlAccountNo, @p_GlType, @p_StartDate,@p_EndDate";

            IEnumerable<MstSppGlAccountDto> result = await _dapperRepo.QueryAsync<MstSppGlAccountDto>(_sql, new
            {
                p_GlAccountNo = input.GlAccountNo,
                p_GlType = input.GlType,
                p_StartDate = input.StartDate,
                p_EndDate = input.EndDate
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
