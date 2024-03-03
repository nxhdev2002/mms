using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.LogW.Pik.Dto;
using prod.Master.LogW;
using prod.Master.LogW.Dto;

namespace prod.LogW.Pik
{
    public class PickingMonitoringScreenAppService : prodAppServiceBase, IPickingMonitoringScreenAppService
    {
        private readonly IRepository<MstLgwPickingTabletProcess, long> _repo;
        private readonly IDapperRepository<MstLgwPickingTabletProcess, long> _dapperRepo;

        public PickingMonitoringScreenAppService(IRepository<MstLgwPickingTabletProcess, long> repo,
                                                IDapperRepository<MstLgwPickingTabletProcess, long> dapperRepo)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
        }

        public async Task<List<object>> GetDataMonitoringScreen(string p_production_line, string p_picking_position_ub, string p_picking_position_sm)
        {
            List<object> list = new List<object>();
            //Screen config
            string _sql1 = "Exec LGW_PIK_PICKING_PROGRESS_MONITOR_GETDATA @p_production_line";
            var data1 = await Task.Run(() => _dapperRepo.QueryAsync<PickingMonitoringScreenDto>(_sql1, new { @p_production_line = p_production_line }));
            var rsPikProgress = from o in data1
                                select new PickingMonitoringScreenDto
                                {
                                    ScreenTitle = o.ScreenTitle,
                                    ProdLine = o.ProdLine,
                                    CurrentTaktTime = o.CurrentTaktTime,
                                    TaktCountDown = o.TaktCountDown,
                                    TotalStop = o.TotalStop,
                                    TotalDelay = o.TotalDelay,
                                    IsPickingDelay = o.IsPickingDelay,
                                    IsAGVAndWDelay = o.IsAGVAndWDelay,
                                    IsLeaderSupport = o.IsLeaderSupport,
                                    PkSmTotal = o.PkSmTotal,
                                    PkUbTotal = o.PkUbTotal,
                                    AgvTotal = o.AgvTotal,
                                    WTotal = o.WTotal
                                };


            string _sql2ub = "Exec LGW_PIK_PICKING_PROGRESS_MONITOR_GET_CUR_PIKDATA @p_production_line,@p_picking_position";
            var data2ub = await Task.Run(() => _dapperRepo.QueryAsync<PickingMonitoringScreenDto>(_sql2ub, new { @p_production_line = p_production_line, @p_picking_position = p_picking_position_ub }));
            var rsCurPikUb = from o in data2ub
                            select new PickingMonitoringScreenDto
                            {
                                ProdLine = o.ProdLine,
                                PickingPosition = o.PickingPosition,
                                LogicSequenceNo = o.LogicSequenceNo,
                                Process = o.Process,
                                LogicSequence = o.LogicSequence,
                                PickingCycle = o.PickingCycle,
                                IsFinished = o.IsFinished
                            };


            string _sql2sm = "Exec LGW_PIK_PICKING_PROGRESS_MONITOR_GET_CUR_PIKDATA @p_production_line,@p_picking_position";
            var data2sm = await Task.Run(() => _dapperRepo.QueryAsync<PickingMonitoringScreenDto>(_sql2sm, new { @p_production_line = p_production_line, @p_picking_position = p_picking_position_sm }));
            var rsCurPikSm = from o in data2sm
                           select new PickingMonitoringScreenDto
                           {
                               ProdLine = o.ProdLine,
                               PickingPosition = o.PickingPosition,
                               LogicSequenceNo = o.LogicSequenceNo,
                               Process = o.Process,
                               LogicSequence = o.LogicSequence,
                               PickingCycle = o.PickingCycle,
                               IsFinished = o.IsFinished
                           };


            string _sql3 = "Exec LGW_PIK_PICKING_PROGRESS_MONITOR_GET_DELAY_PIKDATA @p_production_line";
            var data3 = await Task.Run(() => _dapperRepo.QueryAsync<PickingMonitoringScreenDto>(_sql3, new { @p_production_line = p_production_line }));
            var rsDelayData = from o in data3
                           select new PickingMonitoringScreenDto
                           {
                               PickingTabletId = o.ScreenTitle,
                               ProdLine = o.ProdLine,
                               ProcessCode = o.ProcessCode,
                               ProcessGroup = o.ProcessGroup,
                               SeqNo = o.SeqNo,
                               WorkingDate = o.WorkingDate,
                               Shift = o.Shift,
                               TaktStartTime = o.TaktStartTime,
                               StartTime = o.StartTime,
                               FinishTime = o.FinishTime,
                               DelayTime = o.DelayTime,
                               IsCurrent = o.IsCurrent,
                               IsDelay = o.IsDelay,
                               IsCallLeader = o.IsCallLeader,
                               IsActive = o.IsActive
                           };



            //Add obj to list
            list.Add(rsPikProgress);
            list.Add(rsCurPikUb);
            list.Add(rsCurPikSm);
            list.Add(rsDelayData);
            return list;
        }
    }
}
