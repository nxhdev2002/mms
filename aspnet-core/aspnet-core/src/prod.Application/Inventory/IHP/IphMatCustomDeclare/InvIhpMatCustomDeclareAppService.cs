using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.IHP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.IHP
{
    [AbpAuthorize(AppPermissions.Pages_DMIHP_Mst_InvIphMatCustomDeclare_View)]
    public class InvIhpMatCustomDeclareAppService : prodAppServiceBase, IInvIhpMatCustomDeclareAppService
    {
        private readonly IDapperRepository<InvIphMatCustomDeclare, long> _dapperRepo;
        private readonly IDapperRepository<InvIphMatCustomDeclareDetails, long> _repo;
        private readonly IInvIhpMatCustomDeclareExporter _calendarListExcelExporter;

        public InvIhpMatCustomDeclareAppService(IDapperRepository<InvIphMatCustomDeclareDetails, long> repo,
                                         IDapperRepository<InvIphMatCustomDeclare, long> dapperRepo,
                                        IInvIhpMatCustomDeclareExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }
        public async Task<PagedResultDto<InvIphMatCustomDeclareDto>> GetAllCustomDeclare(GetInvIphMatCustomDeclareInput input)
        {
            string _sql = "Exec INV_IHP_MAT_CUSTOMSDECLARE_SEARCH @p_SOTK, @p_NGAY_DK_FROM, @p_NGAY_DK_TO , @p_SO_HDTM,@p_VAN_DON,@p_PART_SPEC  ";

            IEnumerable<InvIphMatCustomDeclareDto> result = await _dapperRepo.QueryAsync<InvIphMatCustomDeclareDto>(_sql,
                  new
                  {
                      p_SOTK = input.SOTK,
                      p_NGAY_DK_FROM = input.NGAY_DK_FROM,
                      p_NGAY_DK_TO = input.NGAY_DK_TO,
                      p_SO_HDTM = input.SO_HDTM,
                      p_VAN_DON = input.VAN_DON,
                      p_PART_SPEC = input.PART_SPEC,
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvIphMatCustomDeclareDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvIphMatCustomDeclareDetailsDto>> GetCustomDeclareDetails(GetInvIphMatCustomDeclareDetailsInput input)
        {
            string _sql = "Exec INV_IHP_MAT_CUSTOMSDECLARE_DETAILS_SEARCH @p_DToKhaiMDID,@p_PART_SPEC ";

            IEnumerable<InvIphMatCustomDeclareDetailsDto> result = await _repo.QueryAsync<InvIphMatCustomDeclareDetailsDto>(_sql,
                  new
                  {
                      p_DToKhaiMDID = input.DToKhaiMDID,
                      p_PART_SPEC = input.PART_SPEC,
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();


            return new PagedResultDto<InvIphMatCustomDeclareDetailsDto>(
                totalCount,
                pagedAndFiltered);
        }
        public async Task<FileDto> GetInvIphMatCustomerDeclareToExcel(GetInvIphMatCustomDeclareInput input)
        {
            string _sql = "Exec INV_IHP_MAT_CUSTOMSDECLARE_SEARCH @p_SOTK, @p_NGAY_DK_FROM, @p_NGAY_DK_TO , @p_SO_HDTM,@p_VAN_DON,@p_PART_SPEC  ";
            IEnumerable<InvIphMatCustomDeclareDto> result = await _dapperRepo.QueryAsync<InvIphMatCustomDeclareDto>(_sql,
                  new
                  {
                      p_SOTK = input.SOTK,
                      p_NGAY_DK_FROM = input.NGAY_DK_FROM,
                      p_NGAY_DK_TO = input.NGAY_DK_TO,
                      p_SO_HDTM = input.SO_HDTM,
                      p_VAN_DON = input.VAN_DON,
                      p_PART_SPEC = input.PART_SPEC,
                  });
            var listPoHeaderResult = result.ToList();
            return _calendarListExcelExporter.CustomDeclareExportToFile(listPoHeaderResult);

        }
        public async Task<FileDto> GetInvIphMatCustomerDeclareDetailsToExcel(GetInvIphMatCustomDeclareDetailsExcelInput input)
        {
            string _sql = "Exec INV_IHP_MAT_CUSTOMSDECLARE_DETAILS_SEARCH @p_DToKhaiMDID,@p_PART_SPEC ";

            IEnumerable<InvIphMatCustomDeclareDetailsDto> result = await _repo.QueryAsync<InvIphMatCustomDeclareDetailsDto>(_sql,
                  new
                  {
                      p_DToKhaiMDID = input.DToKhaiMDID,
                      p_PART_SPEC = input.PART_SPEC,
                  });

            var listResult = result.ToList();
            return _calendarListExcelExporter.CustomDeclareDetailsExportToFile(listResult);

        }
    }
}
