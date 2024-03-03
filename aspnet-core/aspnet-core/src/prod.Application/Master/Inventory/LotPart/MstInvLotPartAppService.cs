using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inventory.LotPart.Dto;
using prod.Master.Inventory.LotPart.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Master.Inventory.LotPart
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Master_LotPart_View)]
    public class MstInvLotPartAppService : prodAppServiceBase, IMstInvLotPartAppService
    {
        private readonly IDapperRepository<MstInvLotPart, long> _dapperRepo;
        private readonly IRepository<MstInvLotPart, long> _repo;
        private readonly IMstInvLotPartExcelExporter _calendarListExcelExporter;

        public MstInvLotPartAppService(IRepository<MstInvLotPart, long> repo,
                                         IDapperRepository<MstInvLotPart, long> dapperRepo,
                                        IMstInvLotPartExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvLotPartDto>> GetAll(GetMstInvLotPartInput input)
        {
            string _sql = "Exec MST_INV_LOT_PART_SEARCH @p_part_no, @p_source, @p_carfamily_code";

            IEnumerable<MstInvLotPartDto> result = await _dapperRepo.QueryAsync<MstInvLotPartDto>(_sql, new
            {

                p_part_no = input.PartNo,

                p_source = input.Source,
                p_carfamily_code = input.CarfamilyCode
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstInvLotPartDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetInvLotPartToExcel(GetMstInvLotPartExportInput input)
        {
            string _sql = "Exec MST_INV_LOT_PART_SEARCH @p_part_no, @p_source, @p_carfamily_code";

            IEnumerable<MstInvLotPartDto> result = await _dapperRepo.QueryAsync<MstInvLotPartDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_source = input.Source,
                p_carfamily_code = input.CarfamilyCode
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
