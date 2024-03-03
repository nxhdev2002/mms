using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace prod.LogW.Mwh
{

	public interface ILgwMwhPxpModuleInputAndonAppService : IApplicationService
	{

		Task<List<GetPxpModuleInputAndonLayoutOutput>> GetPxpModuleInputAndonLayout(string zone, string screen_id);
		Task<List<GetPxpModuleInputAndonDataOutput>> GetPxpModuleInputAndonData(string zone, string screen_id);
		Task<List<GetPxpModuleCaseListOutput>> GetPxpModuleCaseList(string p_case_type);
		Task<List<GetPxpModuleSuggestionListOutput>> GetPxpModuleSuggestionList(string p_case_type);
		Task<List<GetPxpModuleLotCodeOutput>> GetPxpModuleLotCode();
		Task<int> PxpModuleInputMoveIn(string p_CASE_NO, string p_SUPPLIER_NO, string p_LOC_ID, DateTime? p_UPDATED_DATE, string p_UPDATED_BY);
		Task<int> PxpModuleInputRobbingMoveIn(string p_RENBAN, string p_SUPPLIER_NO, string p_COLUMN);
		Task<int> PxpModuleInputUnpackingCall(string p_caseno, string p_renban, string p_supplier_no);
		Task<List<GetPxpModuleCaseNoLocationOutput>> GetPxpModuleCaseNoLocation();
		Task<int> PxpModuleInputRobbingMoveOutDelete(string p_caseno, string p_renban, string p_SUPPLIER_NO);
		Task<int> PxpModuleInputRobbingCaseMoveUP(string p_caseno, string p_renban, string p_SUPPLIER_NO);
		Task<int> PxpModuleInputCaseMoveUp(string p_caseno, string p_SUPPLIER_NO, DateTime? p_UPDATED_DATE, string p_UPDATED_BY);
		Task<int> PxpModuleInputCaseDelete(string p_caseno, string p_SUPPLIER_NO, string p_LOC_ID, DateTime? p_UPDATED_DATE, string p_UPDATED_BY);
	}

}


