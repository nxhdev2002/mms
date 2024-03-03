using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Inventory.DRM.StockRundown.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.DRM
{
    [AbpAuthorize(AppPermissions.Pages_DMIHP_Rundown_StockRundown_View)]
    public class InvDrmStockRundownAppService : prodAppServiceBase, IInvDrmStockRundownAppService
    {
        private readonly IDapperRepository<InvDrmStockRundown, long> _dapperRepo;

        public InvDrmStockRundownAppService(IDapperRepository<InvDrmStockRundown, long> dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }

        public async Task<PagedResultDto<InvDrmStockRundownDto>> GetAll(GetInvDrmStockRundownInput input)
        {
            string _sql = "Exec INV_DRM_STOCK_RUNDOWN_SEARCH @p_material_code, @p_material_spec";

            IEnumerable<InvDrmStockRundownDto> result = await _dapperRepo.QueryAsync<InvDrmStockRundownDto>(_sql, new
            {
                p_material_code = input.MaterialCode,
                p_material_spec = input.MaterialSpec
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvDrmStockRundownDto>(
                totalCount,
                pagedAndFiltered);

        }

        [AbpAuthorize(AppPermissions.Pages_DMIHP_Rundown_StockRundown_Calculator)]
        public async Task<int> CalculatorRundown()
        {
            try
            {
                string _sql = @"EXEC JOB_INV_DRM_STOCK_RUNDOWN_CREATE @p_date";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    @p_date = DateTime.Now
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

    }
}
