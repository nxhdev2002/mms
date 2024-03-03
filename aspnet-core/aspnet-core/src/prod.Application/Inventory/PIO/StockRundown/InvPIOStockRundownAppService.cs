using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Inventory.PIO.StockRundown.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.StockRundown
{

    [AbpAuthorize(AppPermissions.Pages_PIO_Warehouse_StockRundown_View)]
    public class InvPIOStockRundownAppService : prodAppServiceBase, IInvPIOStockRundownAppService
    {
        private readonly IDapperRepository<InvPIOStockRundown, long> _dapperRepo;

        public InvPIOStockRundownAppService(IDapperRepository<InvPIOStockRundown, long> dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }

        public async Task<PagedResultDto<InvPIOStockRundownDto>> GetAll(GetInvPIOStockRundownInput input)
        {
            string _sql = "Exec INV_PIO_STOCK_RUNDOWN_SEARCH @p_PartNo, @p_MktCode";

            IEnumerable<InvPIOStockRundownDto> result = await _dapperRepo.QueryAsync<InvPIOStockRundownDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_MktCode = input.MktCode,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPIOStockRundownDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task CalculatorPioRundown(DateTime DateCal)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _calculator = "Exec JOB_INV_PIO_STOCK_RUNDOWN_CREATE @datecal";
            await _dapperRepo.QueryAsync<InvPIOStockRundownDto>(_calculator, new { datecal = DateCal });
        }
    }
}
