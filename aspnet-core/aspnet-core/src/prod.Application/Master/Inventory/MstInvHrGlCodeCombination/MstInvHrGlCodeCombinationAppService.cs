using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Hr_HrGlcodeCombination_View)]
    public class MstInvHrGlCodeCombinationAppService : prodAppServiceBase, IMstInvHrGlCodeCombinationAppService
    {
        private readonly IDapperRepository<MstInvHrGlCodeCombination, long> _dapperRepo;
        private readonly IRepository<MstInvHrGlCodeCombination, long> _repo;
        private readonly IMstInvHrGlCodeCombinationExcelExporter _calendarListExcelExporter;

        public MstInvHrGlCodeCombinationAppService(IRepository<MstInvHrGlCodeCombination, long> repo,
                                         IDapperRepository<MstInvHrGlCodeCombination, long> dapperRepo,
                                        IMstInvHrGlCodeCombinationExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvHrGlCodeCombinationDto>> GetAll(GetMstInvHrGlCodeCombinationInput input)
        {
            var filtered = _repo.GetAll()
                /*     .WhereIf(!string.IsNullOrWhiteSpace(input.AccountType), e => e.AccountType.Contains(input.AccountType))*/
                .WhereIf(!string.IsNullOrWhiteSpace(input.Concatenatedsegments), e => e.Concatenatedsegments.Contains(input.Concatenatedsegments));


            var pageAndFiltered = filtered.OrderBy(s => s.Concatenatedsegments);


            var system = from o in pageAndFiltered
                         select new MstInvHrGlCodeCombinationDto
                         {
                             Id = o.Id,
                             ChartOfAccountsId = o.ChartOfAccountsId,
                             AccountType = o.AccountType,
                             EnabledFlag = o.EnabledFlag,
                             Segment1 = o.Segment1,
                             Segment2 = o.Segment2,
                             Segment3 = o.Segment3,
                             Segment4 = o.Segment4,
                             Segment5 = o.Segment5,
                             Segment6 = o.Segment6,
                             Concatenatedsegments = o.Concatenatedsegments,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvHrGlCodeCombinationDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetHrGlCodeCombinationToExcel(MstInvHrGlCodeCombinationExportInput input)
        {
            var filtered = _repo.GetAll()
                /*   .WhereIf(!string.IsNullOrWhiteSpace(input.AccountType), e => e.AccountType.Contains(input.AccountType))*/
                .WhereIf(!string.IsNullOrWhiteSpace(input.Concatenatedsegments), e => e.Concatenatedsegments.Contains(input.Concatenatedsegments));


            var pageAndFiltered = filtered.OrderBy(s => s.Concatenatedsegments);

            var query = from o in pageAndFiltered
                        select new MstInvHrGlCodeCombinationDto
                        {
                            Id = o.Id,
                            ChartOfAccountsId = o.ChartOfAccountsId,
                            AccountType = o.AccountType,
                            EnabledFlag = o.EnabledFlag,
                            Segment1 = o.Segment1,
                            Segment2 = o.Segment2,
                            Segment3 = o.Segment3,
                            Segment4 = o.Segment4,
                            Segment5 = o.Segment5,
                            Segment6 = o.Segment6,
                            Concatenatedsegments = o.Concatenatedsegments,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}