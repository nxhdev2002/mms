using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.LogW.Mwh

{
	public interface IStockAppService : IApplicationService
	{
        Task<List<object>> StockGetData(string p_screen_mode);

        Task<List<object>> StockGetDataLoadForm();
    }
}


