using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prod.LogA.Bp2.Dto;
using prod.Master.Common.Dto;

namespace prod.LogA.Bp2
{
    public class LgaBp2BigPartDDLAppService : prodAppServiceBase, ILgaBp2BigPartDDLAppServices
    {
        private readonly IDapperRepository<LgaBp2PxPUpPlan, long> _dapperRepo;

        public LgaBp2BigPartDDLAppService(IDapperRepository<LgaBp2PxPUpPlan, long> dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }

        public async Task<PagedResultDto<LgaBp2BigPartDDpDto>> GetDataBigPartDDL(string prod_line)
        {
            string _sql = "Exec LGA_BP2_BIG_PART_PROGRESS_GET_DATA @prod_line";

            var filtered = await _dapperRepo.QueryAsync<LgaBp2BigPartDDpDto>(_sql, new { @prod_line = prod_line });
            var results = from o in filtered
                          select new LgaBp2BigPartDDpDto
                          {
                              Title = o.Title,                              
                              ProdLine = o.ProdLine,
                              WorkingDate = o.WorkingDate,
                              Shift = o.Shift,
                              EcarName = o.EcarName,
                              Code = o.Code,
                              ProcessName = o.ProcessName,
                              Sorting = o.Sorting,
                              ActualVolCount = o.ActualVolCount,
                              PlanVolCount = o.PlanVolCount,
                              DelaySecond = o.DelaySecond,
                              ActualTrim = o.ActualTrim,
                              PlanTrim = o.PlanTrim,
                              StartDatetime = o.StartDatetime,
                              FinishDatetime = o.FinishDatetime,
                              TotalTaktTime = o.TotalTaktTime,
                              ProcessTaktTime = o.ProcessTaktTime,
                              ProcessActualTime = o.ProcessActualTime,
                              NewTaktStartTime = o.NewTaktStartTime,
                              IsDelayStart = o.IsDelayStart,
                              ScreenStatus = o.ScreenStatus,
                              Status = o.Status,
                              NTSignalCount = o.NTSignalCount,
                              DelayStartSecond = o.DelayStartSecond,
                              NewTaktOffset = o.NewTaktOffset,
                              RemainingTime = o.RemainingTime,
                              EcarCount = o.EcarCount,

                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<LgaBp2BigPartDDpDto>(
                totalCount,
                results.ToList()
            );
        }

        public async Task<List<BigPartTabletAndonOutput>> GetDataBigPartTabletAndon(string p_Line, string p_ecar_id)
        {
            string _sql = "Exec LGA_BP2_BIG_PART_TABLET_GET_DATA @prod_line, @ecar_id";

            var filtered = await _dapperRepo.QueryAsync<BigPartTabletAndonOutput>(_sql, new
            {
                prod_line = p_Line,
                ecar_id = p_ecar_id
            });

            return filtered.ToList();
        }

        public async Task<List<BigPartTabletEcarOutput>> GetDataBigPartEcarAndon()
        {
            string _sql = "Exec MST_LGA_BP2_ECAR_GET_DATA";

            var filtered = await _dapperRepo.QueryAsync<BigPartTabletEcarOutput>(_sql, new { } );

            return filtered.ToList();
        }
        public async Task<int> BigPartTabletAndon_START_FINISH(string p_progress_id)
        {
            try
            {
                string _sql = @"EXEC LGA_BP2_BIG_PART_TABLET_START_FINISH @p_progress_id";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_progress_id = p_progress_id
                });
                return filtered;
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }

        public async Task<int> BigPartTabletAndon_CALL_LEADER(string p_progress_id)
        {
            try
            {
                string _sql = @"EXEC LGA_BP2_BIG_PART_TABLET_CALL_LEADER @p_progress_id";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_progress_id = p_progress_id
                });
                return filtered;
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }

        public async Task<int> BigPartTabletAndon_UNDO(string p_progress_id)
        {
            try
            {
                string _sql = @"EXEC LGA_BP2_BIG_PART_TABLET_UNDO @p_progress_id";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_progress_id = p_progress_id
                });
                return filtered;
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }

        public async Task<MstCmmLookupDto> GetItemValue(string p_DomainCode, string p_ItemCode)
        {

            string _sql = "Exec MST_CMN_LOOKUP_GET_ITEM_VALUE @DomainCode, @ItemCode";

            var filtered = await _dapperRepo.QueryAsync<MstCmmLookupDto>(_sql, new
            {
                DomainCode = p_DomainCode,
                ItemCode = p_ItemCode
            });

            return filtered.FirstOrDefault();
        }

        public async Task<int> BigPartTabletAndon_NEXTC(string p_prod_line, string p_ecar_id)
        {
            try
            {
                string _sql = @"EXEC LGA_BP2_BIG_PART_TABLET_NEXTC @prod_line, @ecar_id";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    @prod_line = p_prod_line,
                    @ecar_id = p_ecar_id
                });
                return filtered;
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }
    }
}
