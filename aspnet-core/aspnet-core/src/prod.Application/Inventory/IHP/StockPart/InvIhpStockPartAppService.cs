using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Inventory.IHP.Dto;
using prod.Inventory.IHP.Exporting;
using prod.Inventory.IHP.StockPart.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IHP.StockPart
{
    [AbpAuthorize(AppPermissions.Pages_DMIHP_Stock_InvIphStockPart_View)]
    public class InvIhpStockPartAppService : prodAppServiceBase, IInvIhpStockPartAppService
    {
        private readonly IDapperRepository<InvIhpStockPart, long> _dapperRepo;
 
        public InvIhpStockPartAppService(
                                         IDapperRepository<InvIhpStockPart, long> dapperRepo )
        {       
            _dapperRepo = dapperRepo;
            
        }

        public async Task<PagedResultDto<InvIhpStockPartDto>> GetAll(GetInvIhpStockPartInput input)
        {

            string _sql = "Exec [INV_IHP_STOCK_PART_SEARCH] @p_PartNo, @p_WorkingDateFrom, @p_WorkingDateTo,@p_Model";


            IEnumerable<InvIhpStockPartDto> result = await _dapperRepo.QueryAsync<InvIhpStockPartDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo,
                p_Model = input.Model
            });
            var listResult = result.ToList();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvIhpStockPartDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<GetInvInpIfViewDto>> GetIFView(GetInvInpIfViewInput input)
        {
            string _sql = "Exec INV_IHP_STOCK_PART_IF_VIEW  @p_WorkingDateFrom, @p_WorkingDateTo";
            IEnumerable<GetInvInpIfViewDto> result = await _dapperRepo.QueryAsync<GetInvInpIfViewDto>(_sql, new
            {
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo
            });
            var listResult = result.ToList();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<GetInvInpIfViewDto>(
                totalCount,
                pagedAndFiltered);
        }


    }
}
