using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.LogW.Dto;
using prod.Master.LogW.Exporting;

namespace prod.Master.LogW
{
    ////  [AbpAuthorize(AppPermissions.Pages_Master_LogW_PickingTabletSetup)]
    public class MstLgwPickingTabletProcessAppService : prodAppServiceBase, IMstLgwPickingTabletProcessAppService
    {
        private readonly IDapperRepository<MstLgwPickingTabletProcess, long> _dapperRepo;
        private readonly IRepository<MstLgwPickingTabletProcess, long> _repo;
        private readonly IMstLgwPickingTabletProcessExcelExporter _calendarListExcelExporter;

        public MstLgwPickingTabletProcessAppService(IRepository<MstLgwPickingTabletProcess, long> repo,
                                         IDapperRepository<MstLgwPickingTabletProcess, long> dapperRepo,
                                        IMstLgwPickingTabletProcessExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        ////  [AbpAuthorize(AppPermissions.Pages_Master_LogW_PickingTabletSetup_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgwPickingTabletProcessDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgwPickingTabletProcessDto input)
        {
           
            var mainObj = ObjectMapper.Map<MstLgwPickingTabletProcess>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgwPickingTabletProcessDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_PickingTabletSetup_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgwPickingTabletProcessDto>> GetAll(GetMstLgwPickingTabletProcessInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PickingTabletId), e => e.PickingTabletId.Contains(input.PickingTabletId))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PickingPosition), e => e.PickingPosition.Contains(input.PickingPosition))
                //.WhereIf(input.PickingCycle != 0, t => t.PickingCycle == input.PickingCycle);
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
     

            var system = from o in pageAndFiltered
                         select new MstLgwPickingTabletProcessDto
                         {
                             Id = o.Id,
                             PickingTabletId = o.PickingTabletId,
                             PickingPosition = o.PickingPosition,
                             Process = o.Process,
                             PickingCycle = o.PickingCycle,
                             LogicSequenceNo = o.LogicSequenceNo,
                             LogicSequence = o.LogicSequence,
                             IsLotSupply = o.IsLotSupply,
                             HasModel = o.HasModel,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwPickingTabletProcessDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<List<MstLgwPickingTabletProcessGetdataOutput>> MstLgwPickingTabletProcessGetdata(string p_pickingposition, string p_tablet_id)
        {

            string _sql = "Exec LGW_PIK_PICKING_PROGRESS_GETDATA @p_picking_position, @p_picking_tablet_id";

            var filtered = await _dapperRepo.QueryAsync<MstLgwPickingTabletProcessGetdataOutput>(_sql, new
            {
                p_picking_position = p_pickingposition,
                p_picking_tablet_id = p_tablet_id
            });

            return filtered.ToList();
        }
        public async Task<List<MstLgwPickingTabletProcessGetdataByLayoutOutput>> MstLgwPickingTabletProcessGetdataByLayout(string p_pickingposition, string p_tablet_id)
        {

            string _sql = "Exec LGW_PIK_PICKING_PROGRESS_GETDATA_BY_LAYOUT @p_picking_position, @p_picking_tablet_id";

            var filtered = await _dapperRepo.QueryAsync<MstLgwPickingTabletProcessGetdataByLayoutOutput>(_sql, new
            {
                p_picking_position = p_pickingposition,
                p_picking_tablet_id = p_tablet_id
            });

            return filtered.ToList();
        }

        public async Task<int> MstLgwPickingTabletProcessStartFinish(string picking_position, string picking_tablet_id)
        {

            string _sql = "Exec LGW_PIK_PICKING_PROGRESS_START_FINISH @p_picking_position, @p_picking_tablet_id";

            int filtered = await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_picking_position = picking_position,
                p_picking_tablet_id = picking_tablet_id
            });

            return filtered;
        }

        public async Task<int> MstLgwPickingTabletProcessCallUPLot(string picking_position, string picking_tablet_id)
        {

            string _sql = "Exec LGW_PIK_PICKING_PROGRESS_CALL_UP_LOT @p_picking_position, @p_picking_tablet_id";

            int filtered = await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_picking_position = picking_position,
                p_picking_tablet_id = picking_tablet_id
            });

            return filtered;
        }

        public async Task<int> MstLgwPickingTabletProcessCallUPPxp(string picking_position, string picking_tablet_id, string back_no)
        {

            string _sql = "Exec LGW_PIK_PICKING_PROGRESS_CALL_UP_PXP @p_picking_position, @p_picking_tablet_id, @p_back_no";

            int filtered = await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_picking_position = picking_position,
                p_picking_tablet_id = picking_tablet_id,
                p_back_no = back_no
            });

            return filtered;
        }


        public async Task<int> MstLgwPickingTabletProcessSendSignal(string picking_position, string logic_sequence_no, string progess_seq_no, string picking_tablet_id)
        {

            string _sql = "Exec LGW_PIK_PICKING_PROGRESS_SEND_SIGNAL @p_picking_position, @p_logic_sequence_no, @p_progess_seq_no, @p_picking_tablet_id ";

            int filtered = await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_picking_position = picking_position,
                p_logic_sequence_no = logic_sequence_no,
                p_progess_seq_no = progess_seq_no,
                p_picking_tablet_id = picking_tablet_id
            });

            return filtered;
        }

        public async Task<int> MstLgwPickingTabletProcessCallLeader(string picking_position, string progess_seq_no, string picking_tablet_id)
        {

            string _sql = "Exec LGW_PIK_PICKING_PROGRESS_CALL_LEADER @p_picking_position, @p_progess_seq_no, @p_picking_tablet_id";

            int filtered = await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_picking_position = picking_position,
                p_progess_seq_no = progess_seq_no,
                p_picking_tablet_id = picking_tablet_id
            });

            return filtered;
        }


        public async Task<FileDto> GetPickingTabletSetupToExcel(MstLgwPickingTabletProcessExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PickingTabletId), e => e.PickingTabletId.Contains(input.PickingTabletId))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PickingPosition), e => e.PickingPosition.Contains(input.PickingPosition))
                //.WhereIf(input.PickingCycle != 0, t => t.PickingCycle == input.PickingCycle);
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwPickingTabletProcessDto
                        {
                            Id = o.Id,
                            PickingTabletId = o.PickingTabletId,
                            PickingPosition = o.PickingPosition,
                            Process = o.Process,
                            PickingCycle = o.PickingCycle,
                            LogicSequenceNo = o.LogicSequenceNo,
                            LogicSequence = o.LogicSequence,
                            IsLotSupply = o.IsLotSupply,
                            HasModel = o.HasModel,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstLgwPickingTabletProcessConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
