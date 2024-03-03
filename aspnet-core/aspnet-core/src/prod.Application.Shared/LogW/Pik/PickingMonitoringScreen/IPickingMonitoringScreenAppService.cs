using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Pik.Dto;

namespace prod.LogW.Pik
{

	public interface IPickingMonitoringScreenAppService : IApplicationService
	{
        Task<List<object>> GetDataMonitoringScreen(string p_production_line, string p_picking_position_ub, string p_picking_position_sm);

    }

}
