using Abp.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks; 

namespace prod.LogW.Mwh
{
   
    public class LgwMwhPxpModuleInputAndonAppService : prodAppServiceBase, ILgwMwhPxpModuleInputAndonAppService
    {
        private readonly IDapperRepository<LgwMwhContList, long> _dapperRepo; 

        public LgwMwhPxpModuleInputAndonAppService(IDapperRepository<LgwMwhContList, long> dapperRepo)
        { 
            _dapperRepo = dapperRepo; 
        }

        public async Task<List<GetPxpModuleInputAndonLayoutOutput>> GetPxpModuleInputAndonLayout(string p_zone, string p_screen_id)
        {

            string _sql = "Exec LGW_MWH_LAYOUT_GET_BY_ACTIVE_SCREEN @zone, @screen_id";

            var filtered = await _dapperRepo.QueryAsync<GetPxpModuleInputAndonLayoutOutput>(_sql, new
            {
                zone = p_zone,
                screen_id = p_screen_id
            });

            return filtered.ToList();
        }

        public async Task<List<GetPxpModuleInputAndonDataOutput>> GetPxpModuleInputAndonData(string p_zone, string p_screen_id)
        {

            string _sql = "Exec LGW_MWH_CASE_DATA_GET_BY_ACTIVE_SCREEN @zone, @screen_id";

            var filtered = await _dapperRepo.QueryAsync<GetPxpModuleInputAndonDataOutput>(_sql, new
            {
                zone = p_zone,
                screen_id = p_screen_id
            });

            return filtered.ToList();
        }

        public async Task<List<GetPxpModuleContRenbanOutput>> GetPxpModuleContRenban(string p_zone, string p_screen_id)
        {

            string _sql = "Exec LGW_MWH_CASE_DATA_GET_BY_ACTIVE_SCREEN @zone, @screen_id";

            var filtered = await _dapperRepo.QueryAsync<GetPxpModuleContRenbanOutput>(_sql, new
            {
                zone = p_zone,
                screen_id = p_screen_id
            });

            List<GetPxpModuleContRenbanOutput> distinctCont = filtered.GroupBy(p => p.ContainerNo).Select(g => g.First()).ToList();

            return distinctCont;
        }


        public async Task<List<GetPxpModuleCaseListOutput>> GetPxpModuleCaseList(string p_case_type)
        {

            string _sql = "Exec LGW_MWH_CASE_DATA_GET_CASE_LIST @case_type";

            var filtered = await _dapperRepo.QueryAsync<GetPxpModuleCaseListOutput>(_sql, new
            {
                case_type = p_case_type
            });

            return filtered.ToList();
        }

        public async Task<List<GetPxpModuleSuggestionListOutput>> GetPxpModuleSuggestionList(string p_case_type)
        {

            string _sql = "Exec LGW_MWH_CASE_DATA_GET_SUGGESTION_LIST @case_type";

            var filtered = await _dapperRepo.QueryAsync<GetPxpModuleSuggestionListOutput>(_sql, new
            {
                case_type = p_case_type
            });

            return filtered.ToList();
        }

        public async Task<List<GetPxpModuleLotCodeOutput>> GetPxpModuleLotCode()
        {

            string _sql = "Exec LGW_MWH_CASE_DATA_GET_LOT_CODE";

            var filtered = await _dapperRepo.QueryAsync<GetPxpModuleLotCodeOutput>(_sql, new   {   });

            return filtered.ToList();
        }

        public async Task<List<GetPxpModuleCaseNoLocationOutput>> GetPxpModuleCaseNoLocation()
        {

            string _sql = "Exec LGW_MWH_CASE_DATA_GET_CASE_NO_LOCATION";

            var filtered = await _dapperRepo.QueryAsync<GetPxpModuleCaseNoLocationOutput>(_sql, new { });

            return filtered.ToList();
        }


        public async Task<int> PxpModuleInputMoveIn(string p_CASE_NO, string p_SUPPLIER_NO, string p_LOC_ID, DateTime? p_UPDATED_DATE, string p_UPDATED_BY)
        {
            try
            { 
                string _sql = @"EXEC LGW_MWH_CASE_DATA_MOVE_IN @CASE_NO, @SUPPLIER_NO, @LOC_ID, @UPDATED_DATE, @UPDATED_BY";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    CASE_NO = p_CASE_NO,
                    SUPPLIER_NO = p_SUPPLIER_NO,
                    LOC_ID = p_LOC_ID,
                    UPDATED_DATE = p_UPDATED_DATE,
                    UPDATED_BY = p_UPDATED_BY
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        public async Task<int> PxpModuleInputRobbingMoveIn(string p_RENBAN, string p_SUPPLIER_NO, string p_COLUMN)
        {
            try
            {
                string _sql = @"EXEC LGW_MWH_CASE_DATA_ROBBING_MOVE_IN @RENBAN, @SUPPLIER_NO, @COLUMN_ID";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    RENBAN = p_RENBAN,
                    SUPPLIER_NO = p_SUPPLIER_NO,
                    COLUMN_ID = p_COLUMN 
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        

        public async Task<int> PxpModuleInputCaseMoveUp(string p_caseno, string p_SUPPLIER_NO, DateTime? p_UPDATED_DATE, string p_UPDATED_BY)
        {
            try
            {
                string _sql = @"EXEC LGW_MWH_CASE_DATA_MOVE_OUT @CASE_NO, @SUPPLIER_NO, @UPDATED_DATE, @UPDATED_BY";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    CASE_NO = p_caseno,
                    SUPPLIER_NO = p_SUPPLIER_NO, 
                    UPDATED_DATE = p_UPDATED_DATE,
                    UPDATED_BY = p_UPDATED_BY
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        public async Task<int> PxpModuleInputCaseDelete(string p_caseno, string p_SUPPLIER_NO, string p_LOC_ID, DateTime? p_UPDATED_DATE, string p_UPDATED_BY)
        {
            try
            {
                string _sql = @"EXEC LGW_MWH_CASE_DATA_DELETE @CASE_NO, @SUPPLIER_NO, @LOC_ID, @UPDATED_DATE, @UPDATED_BY";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    CASE_NO = p_caseno, 
                    SUPPLIER_NO = p_SUPPLIER_NO,
                    LOC_ID = p_LOC_ID,
                    UPDATED_DATE = p_UPDATED_DATE,
                    UPDATED_BY = p_UPDATED_BY
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        public async Task<int> PxpModuleInputRobbingMoveOutDelete(string p_caseno, string p_renban, string p_SUPPLIER_NO)
        {
            try
            {
                string _sql = @"EXEC LGW_MWH_CASE_DATA_ROBBING_CASE_DELETE @CASE_NO, @RENBAN, @SUPPLIER_NO";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    CASE_NO = p_caseno,
                    RENBAN = p_renban,
                    SUPPLIER_NO = p_SUPPLIER_NO
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        public async Task<int> PxpModuleInputRobbingCaseMoveUP(string p_caseno, string p_renban, string p_supplier_no)
        {
            try
            {
                string _sql = @"EXEC LGW_MWH_CASE_DATA_ROBBING_CASE_MOVEUP @CASE_NO, @RENBAN, @SUPPLIER_NO";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    CASE_NO = p_caseno,
                    RENBAN = p_renban,
                    SUPPLIER_NO = p_supplier_no
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        public async Task<int> PxpModuleInputUnpackingCall(string p_caseno, string p_renban, string p_supplier_no)
        {
            try
            {
                string _sql = @"EXEC LGW_MWH_CASE_DATA_UNPACKING_CALL @CASE_NO, @RENBAN, @SUPPLIER_NO";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    CASE_NO = p_caseno,
                    RENBAN = p_renban,
                    SUPPLIER_NO = p_supplier_no
                });
                return filtered;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

    }
}
