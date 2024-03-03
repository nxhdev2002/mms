using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.Gps.Mapping.Dto;
using prod.Inventory.Gps.Mapping.Exporting;
using prod.Inventory.GPS;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.Mapping
{
    [AbpAuthorize(AppPermissions.Pages_GPS_Issuing_Mapping_View)]
    public class InvGpsMappingAppService : prodAppServiceBase, IInvGpsMappingAppService
    {
        private readonly IDapperRepository<InvGpsMapping, long> _dapperRepo;
        private readonly IInvGpsMappingExcelExporter _excelExporter;

        public InvGpsMappingAppService(IDapperRepository<InvGpsMapping, long> dapperRepo,
                                       IInvGpsMappingExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }
        public async Task<PagedResultDto<InvGpsMappingDto>> GetAll(GetInvGpsMappingInput input)
        {
            string _sql = "Exec INV_GPS_STOCK_MAPPING_SEARCH @p_PartNo, @p_PartCategory, @p_ShopRegister, @p_CostCenter, @p_Wbs, @p_GlAccount";

            IEnumerable<InvGpsMappingDto> result = await _dapperRepo.QueryAsync<InvGpsMappingDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_PartCategory = input.PartCatetory,
                p_ShopRegister = input.ShopRegister,
                p_CostCenter = input.CostCenter,
                p_Wbs = input.Wbs,
                p_GlAccount = input.GlAccount
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsMappingDto>(totalCount, pagedAndFiltered);
        }


        public async Task<FileDto> GetInvGpsMappingToExcel(GetInvGpsMappingExportInput input)
        {
            string _sql = "Exec INV_GPS_STOCK_MAPPING_SEARCH @p_PartNo, @p_PartCategory, @p_ShopRegister, @p_CostCenter, @p_Wbs, @p_GlAccount";

            IEnumerable<InvGpsMappingDto> result = await _dapperRepo.QueryAsync<InvGpsMappingDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_PartCategory = input.PartCatetory,
                p_ShopRegister = input.ShopRegister,
                p_CostCenter = input.CostCenter,
                p_Wbs = input.Wbs,
                p_GlAccount = input.GlAccount
            });

            var exportToExcel = result.ToList();
            return _excelExporter.ExportToFile(exportToExcel);
        }

        [AbpAuthorize(AppPermissions.Pages_GPS_Issuing_Mapping_ReMap)]
        public async Task ReMapInvGpsMapping()
        {
            string _sql = "Exec INV_GPS_MAPPING_REMAP ";

            await _dapperRepo.ExecuteAsync(_sql, new { });
        }


        public async Task ValidateInvGpsMapping()
        {
            string _sql = "Exec INV_GPS_MAPPING_VAIDATE ";

            await _dapperRepo.ExecuteAsync(_sql, new { });
        }
    }
}
