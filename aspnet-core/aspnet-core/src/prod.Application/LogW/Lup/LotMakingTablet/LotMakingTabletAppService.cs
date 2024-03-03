using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.LogW.Lup.Dto;

namespace prod.LogW.Lup
{
    public class LotMakingTabletAppService : prodAppServiceBase, ILotMakingTabletAppService
    {
        private readonly IDapperRepository<LgwLupLotUpPlan, long> _dapperRepo;

        public LotMakingTabletAppService(IDapperRepository<LgwLupLotUpPlan, long> dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }

        public async Task<PagedResultDto<LotMakingTabletDto>> GetMkDataLotUpPlan()
        {
            string _sql = "Exec LGW_LUP_LOT_UP_PLAN_GET_MK_DATA";

            var filtered = await _dapperRepo.QueryAsync<LotMakingTabletDto>(_sql, new {});
            var results = from o in filtered
                          select new LotMakingTabletDto
                          {
                              SeqNo = o.SeqNo,
                              Id = o.Id,
                              ProdLine = o.ProdLine,
                              WorkingDate = o.WorkingDate,
                              Shift = o.Shift,
                              LotNo = o.LotNo,
                              PlanUnpackingStartDatetime = o.PlanUnpackingStartDatetime,
                              PlanUnpackingFinishDatetime = o.PlanUnpackingFinishDatetime,
                              Tpm = o.Tpm,
                              Remarks = o.Remarks,
                              UpCalltime = o.UpCalltime,
                              UnpackingActualStartDatetime = o.UnpackingActualStartDatetime,
                              UnpackingActualFinishDatetime = o.UnpackingActualFinishDatetime,
                              UpStatus = o.UpStatus,
                              MakingFinishDatetime = o.MakingFinishDatetime,
                              MkStatus = o.MkStatus,
                              Status = o.Status,
                              MkModuleCount = o.MkModuleCount,
                              MaxLot = o.MaxLot,
                              ScreenStatus = o.ScreenStatus
                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<LotMakingTabletDto>(
                totalCount, 
                results.ToList()
            );
        }

        public async Task<PagedResultDto<LotMakingTabletDto>> GetMkModuleListDataLotUpPlan(string prod_line, string lot_no, int lot_id)
        {
            string _sql = "Exec LGW_LUP_LOT_UP_PLAN_GET_MK_MODULE_LIST @p_prod_line, @p_lot_no, @p_lot_id";

            var filtered = await _dapperRepo.QueryAsync<LotMakingTabletDto>(_sql, new
            {
                p_prod_line = prod_line,
                p_lot_no = lot_no,
                p_lot_id = lot_id
            });
            var results = from o in filtered
                          select new LotMakingTabletDto
                          {
                              CaseNo = o.CaseNo,
                              SupplierNo = o.SupplierNo
                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<LotMakingTabletDto>(
                totalCount,
                results.ToList()
            );
        }

        public async Task FinishLotMk(int lot_id)
        {
            string _sql = "Exec LGW_LUP_LOT_UP_PLAN_FINISH_LOT_MK @p_lot_id";
            await _dapperRepo.QueryAsync<LgwLupLotUpPlan>(_sql, new { p_lot_id = lot_id });
        }
    }
}
