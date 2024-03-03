using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inventory.HrTitles.Dto;
using prod.Master.Inventory.HrTitles.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Master.Inventory.HrTitles
{
    [AbpAuthorize(AppPermissions.Pages_Master_Hr_HrTitles_View)]
    public class MstInvHrTitlesAppService : prodAppServiceBase, IMstInvHrTitlesAppService
    {
        private readonly IDapperRepository<MstInvHrTitles, long> _dapperRepo;
        private readonly IRepository<MstInvHrTitles, long> _repo;
        private readonly IMstInvHrTitlesExcelExporter _calendarListExcelExporter;

        public MstInvHrTitlesAppService(IRepository<MstInvHrTitles, long> repo,
                                         IDapperRepository<MstInvHrTitles, long> dapperRepo,
                                        IMstInvHrTitlesExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvHrTitlesDto>> GetAll(MstInvHrTitlesInput input)
        {
            string _sql = "Exec MST_INV_HR_TITLES_SEARCH ";

            IEnumerable<MstInvHrTitlesDto> result = await _dapperRepo.QueryAsync<MstInvHrTitlesDto>(_sql, new
            {
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstInvHrTitlesDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetMstInvHrTitlesToExcel(MstInvHrTitlesExportInput input)
        {

            string _sql = "Exec MST_INV_HR_TITLES_SEARCH ";

            IEnumerable<MstInvHrTitlesDto> result = await _dapperRepo.QueryAsync<MstInvHrTitlesDto>(_sql, new
            {
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
