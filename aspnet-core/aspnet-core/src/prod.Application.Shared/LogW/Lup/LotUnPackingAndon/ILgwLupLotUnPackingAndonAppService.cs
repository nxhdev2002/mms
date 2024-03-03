using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace prod.LogW.Lup
{

	public interface ILgwLupLotUnPackingAndonAppService : IApplicationService
	{
		Task<List<GetLotUnPackingAndonOutput>> LgwLupLotUnPackingAndonGetData(string p_Line);  

	}

}


