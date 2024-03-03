using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Hr_HrPosition_View)]
    public class MstInvHrPositionAppService : prodAppServiceBase, IMstInvHrPositionAppService
    {
        private readonly IDapperRepository<MstInvHrPosition, long> _dapperRepo;
        private readonly IRepository<MstInvHrPosition, long> _repo;
        private readonly IMstInvHrPositionExcelExporter _calendarListExcelExporter;

        public MstInvHrPositionAppService(IRepository<MstInvHrPosition, long> repo,
                                         IDapperRepository<MstInvHrPosition, long> dapperRepo,
                                        IMstInvHrPositionExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvHrPositionDto>> GetAll(GetMstInvHrPositionInput input)
        {
            string _sql = "Exec MST_INV_HR_POSITION_SEARCH ";

            IEnumerable<MstInvHrPositionDto> result = await _dapperRepo.QueryAsync<MstInvHrPositionDto>(_sql, new
            {
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstInvHrPositionDto>(
                totalCount,
                pagedAndFiltered);
        }

        /*            public async Task<FileDto> GetHrPositionToExcel(MstInvHrPositionExportInput input)
                {
                    var query = from o in _repo.GetAll()
                                select new MstInvHrPositionDto
                                {
                                    Id = o.Id,
                                    Code = o.Code,
                                    Name = o.Name,
                                    Description = o.Description,
                                    IsActive = o.IsActive,
                                };
                    var exportToExcel = await query.ToListAsync();
                    return _calendarListExcelExporter.ExportToFile(exportToExcel);
                }*/

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstInvHrPositionConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
