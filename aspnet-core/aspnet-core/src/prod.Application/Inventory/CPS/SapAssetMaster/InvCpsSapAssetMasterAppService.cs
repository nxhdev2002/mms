using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Inventory.CPS.SapAssetMaster.Dto;
using prod.Inventory.CPS.SapAssetMaster.Exporting;
using prod.Master.Inventory;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Inventory.CPS.Dto;
using prod.Dto;
using prod.Inventory.SPP.Shipping.Dto;
using NPOI.SS.Formula.Functions;

namespace prod.Inventory.CPS.SapAssetMaster
{
    [AbpAuthorize(AppPermissions.Pages_CpsLinkAge_SapAssetMaster_View)]
    public class InvCpsSapAssetMasterAppService : prodAppServiceBase, IInvCpsSapAssetMasterAppService
    {
        private readonly IDapperRepository<InvCpsSapAssetMaster, long> _dapperRepo;
        private readonly IRepository<InvCpsSapAssetMaster, long> _repo;
        private readonly IInvCpsSapAssetMasterExcelExporter _cpsSapAssetMasterExcelExporter;
        public InvCpsSapAssetMasterAppService(IRepository<InvCpsSapAssetMaster, long> repo,
                                         IDapperRepository<InvCpsSapAssetMaster, long> dapperRepo,
                                         IInvCpsSapAssetMasterExcelExporter cpsSapAssetMasterExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _cpsSapAssetMasterExcelExporter = cpsSapAssetMasterExcelExporter;
        }

        public async Task<PagedResultDto<InvCpsSapAssetMasterDto>> GetAll(GetInvCpsSapAssetMasterInput input)
        {
            string _sqlSearch = "Exec INV_CPS_SAP_ASSET_MASTER_SEARCH @p_companycode, @p_fixedassetnumber, @p_serialnumber, @p_wbs, @p_costcenter";

            IEnumerable<InvCpsSapAssetMasterDto> result = await _dapperRepo.QueryAsync<InvCpsSapAssetMasterDto>(_sqlSearch,
                  new
                  {
                      p_companycode = input.CompanyCode,
                      p_fixedassetnumber = input.FixedAssetNumber,
                      p_serialnumber = input.SerialNumber,
                      p_wbs = input.WBS,
                      p_costcenter = input.CostCenter
                  });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCpsSapAssetMasterDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetSapAssetMasterToExcel(GetInvCpsSapAssetMasterExportInput input)
        {
            string _sqlSearch = "Exec INV_CPS_SAP_ASSET_MASTER_SEARCH @p_companycode, @p_fixedassetnumber, @p_serialnumber, @p_wbs, @p_costcenter";

            IEnumerable<InvCpsSapAssetMasterDto> result = await _dapperRepo.QueryAsync<InvCpsSapAssetMasterDto>(_sqlSearch,
                  new
                  {
                      p_companycode = input.CompanyCode,
                      p_fixedassetnumber = input.FixedAssetNumber,
                      p_serialnumber = input.SerialNumber,
                      p_wbs = input.WBS,
                      p_costcenter = input.CostCenter
                  });

            var exportToExcel = result.ToList();
            return _cpsSapAssetMasterExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
