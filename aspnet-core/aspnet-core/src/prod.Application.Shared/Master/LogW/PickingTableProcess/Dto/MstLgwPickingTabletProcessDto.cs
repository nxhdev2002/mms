using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwPickingTabletProcessDto : EntityDto<long?>
	{
        public virtual string logicSequenceisLotSupply { get; set; }
        public virtual string PickingTabletId { get; set; }

		public virtual string PickingPosition { get; set; }

		public virtual string Process { get; set; }

		public virtual int? PickingCycle { get; set; }

		public virtual int? LogicSequenceNo { get; set; }

		public virtual string LogicSequence { get; set; }

		public virtual string IsLotSupply { get; set; }

		public virtual string HasModel { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgwPickingTabletProcessDto : EntityDto<long?>
	{
		public virtual string logicSequenceisLotSupply { get; set; }
		public virtual string PickingTabletId { get; set; }

		[StringLength(MstLgwPickingTabletProcessConsts.MaxPickingPositionLength)]
		public virtual string PickingPosition { get; set; }

		[StringLength(MstLgwPickingTabletProcessConsts.MaxProcessLength)]
		public virtual string Process { get; set; }

		public virtual int? PickingCycle { get; set; }

		public virtual int? LogicSequenceNo { get; set; }

		[StringLength(MstLgwPickingTabletProcessConsts.MaxLogicSequenceLength)]
		public virtual string LogicSequence { get; set; }

		[StringLength(MstLgwPickingTabletProcessConsts.MaxIsLotSupplyLength)]
		public virtual string IsLotSupply { get; set; }

		[StringLength(MstLgwPickingTabletProcessConsts.MaxHasModelLength)]
		public virtual string HasModel { get; set; }

		[StringLength(MstLgwPickingTabletProcessConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstLgwPickingTabletProcessInput : PagedAndSortedResultRequestDto
	{
		public virtual string PickingTabletId { get; set; }

		public virtual string PickingPosition { get; set; }

		public virtual string Process { get; set; }

		public virtual int? PickingCycle { get; set; }

		public virtual string LogicSequence { get; set; }

		public virtual string IsLotSupply { get; set; }

		public virtual string HasModel { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class MstLgwPickingTabletProcessGetdataOutput : EntityDto<long?>
	{
		public virtual string logicSequenceisLotSupply { get; set; }

		public virtual string PickingPosition { get; set; }
		public virtual string PickingTabletId { get; set; }
		public virtual int? LogicSequenceNo { get; set; }
		public virtual string Process { get; set; }
		public virtual string LogicSequence { get; set; }
		public virtual int? PickingCycle { get; set; }
		public virtual string IsPushed { get; set; }
		public virtual string HasModel { get; set; }
		public virtual int? TaktTime { get; set; }

		public virtual DateTime? TaktStartTime { get; set; }
		public virtual DateTime? CurrentTime { get; set; }
		public virtual DateTime? StartTime { get; set; }
		public virtual DateTime? FinishTime { get; set; }

		public virtual string SeqNo { get; set; }
		public virtual DateTime? WorkingDate { get; set; }
		public virtual string Shift { get; set; }
		public virtual string Model { get; set; }
		public virtual string LotCode { get; set; }
		public virtual string CanCallUnpacking { get; set; }
		public virtual string IsCallLeader { get; set; }
	}

	public class MstLgwPickingTabletProcessGetdataByLayoutOutput : EntityDto<long?>
	{
		//public virtual string logicSequenceisLotSupply { get; set; }
		public virtual int? RowNo { get; set; }
		public virtual int? ColNo { get; set; }

		public virtual string PickingPosition { get; set; }
		public virtual string PickingTabletId { get; set; }
		public virtual int? LogicSequenceNo { get; set; }
		public virtual string Process { get; set; }
		public virtual string LogicSequence { get; set; }
		public virtual int? PickingCycle { get; set; }
		public virtual string IsPushed { get; set; }
		public virtual string HasModel { get; set; }
		public virtual int? TaktTime { get; set; }

		public virtual DateTime? TaktStartTime { get; set; }
		public virtual DateTime? CurrentTime { get; set; }
		public virtual DateTime? StartTime { get; set; }
		public virtual DateTime? FinishTime { get; set; }

		public virtual string SeqNo { get; set; }
		public virtual DateTime? WorkingDate { get; set; }
		public virtual string Shift { get; set; }
		public virtual string Model { get; set; }
		public virtual string LotCode { get; set; }
		public virtual string CanCallUnpacking { get; set; }
		public virtual string IsCallLeader { get; set; }

		public virtual long? TabletProcessId { get; set; }
		public virtual long? PickingProcessId { get; set; }
		public virtual string LblLogicSequence { get; set; }
		public virtual string LblProcess { get; set; }
		public virtual long? LblProcessId { get; set; }
		public virtual string LblIsPushed { get; set; }
		public virtual string LblIsHidden { get; set; }

		public virtual int? ArrowDirection { get; set; }
		


	}

}


