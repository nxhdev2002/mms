using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW
{

	public interface IMstLgwPickingTabletProcessAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgwPickingTabletProcessDto>> GetAll(GetMstLgwPickingTabletProcessInput input);

		Task CreateOrEdit(CreateOrEditMstLgwPickingTabletProcessDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPickingTabletSetupToExcel(MstLgwPickingTabletProcessExportInput input);

        Task<List<MstLgwPickingTabletProcessGetdataOutput>> MstLgwPickingTabletProcessGetdata(string p_pickingposition, string p_tablet_id);
		Task<List<MstLgwPickingTabletProcessGetdataByLayoutOutput>> MstLgwPickingTabletProcessGetdataByLayout(string p_pickingposition, string p_tablet_id);

		Task<int> MstLgwPickingTabletProcessStartFinish(string picking_position, string picking_tablet_id);
		Task<int> MstLgwPickingTabletProcessSendSignal(string picking_position, string logic_sequence_no, string progess_seq_no, string picking_tablet_id);
		Task<int> MstLgwPickingTabletProcessCallLeader(string picking_position, string progess_seq_no, string picking_tablet_id);
		Task<int> MstLgwPickingTabletProcessCallUPLot(string picking_position, string picking_tablet_id);
		Task<int> MstLgwPickingTabletProcessCallUPPxp(string picking_position, string picking_tablet_id, string back_no);
	}

}


