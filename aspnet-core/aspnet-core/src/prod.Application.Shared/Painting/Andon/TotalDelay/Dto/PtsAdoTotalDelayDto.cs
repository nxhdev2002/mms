using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoTotalDelayDto : EntityDto<long?>
	{

		public virtual long? WipId { get; set; }

		public virtual long? ProgressId { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string Color { get; set; }

		public virtual string Mode { get; set; }

		public virtual string TargetRepair { get; set; }

		public virtual DateTime? StartRepair { get; set; }

		public virtual string Location { get; set; }

		public virtual DateTime? AInPlanDate { get; set; }

		public virtual DateTime? EdInAct { get; set; }

		public virtual DateTime? RepairIn { get; set; }

		public virtual DateTime? Leadtime { get; set; }

		public virtual DateTime? LeadtimePlus { get; set; }

		public virtual DateTime? Etd { get; set; }

		public virtual DateTime? RecoatIn { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditPtsAdoTotalDelayDto : EntityDto<long?>
	{

		public virtual long? WipId { get; set; }

		public virtual long? ProgressId { get; set; }

		[StringLength(PtsAdoTotalDelayConsts.MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(PtsAdoTotalDelayConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(PtsAdoTotalDelayConsts.MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(PtsAdoTotalDelayConsts.MaxModeLength)]
		public virtual string Mode { get; set; }

		[StringLength(PtsAdoTotalDelayConsts.MaxTargetRepairLength)]
		public virtual string TargetRepair { get; set; }

		public virtual DateTime? StartRepair { get; set; }

		[StringLength(PtsAdoTotalDelayConsts.MaxLocationLength)]
		public virtual string Location { get; set; }

		public virtual DateTime? AInPlanDate { get; set; }

		public virtual DateTime? EdInAct { get; set; }

		public virtual DateTime? RepairIn { get; set; }

		public virtual DateTime? Leadtime { get; set; }

		public virtual DateTime? LeadtimePlus { get; set; }

		public virtual DateTime? Etd { get; set; }

		public virtual DateTime? RecoatIn { get; set; }

		[StringLength(PtsAdoTotalDelayConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetPtsAdoTotalDelayInput : PagedAndSortedResultRequestDto
	{

		public virtual string BodyNo { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string Color { get; set; }

		public virtual string Mode { get; set; }

		public virtual string TargetRepair { get; set; }

		public virtual DateTime? StartRepair { get; set; }

		public virtual string Location { get; set; }

		public virtual DateTime? AInPlanDate { get; set; }

		public virtual DateTime? EdInAct { get; set; }

		public virtual DateTime? RepairIn { get; set; }

		public virtual DateTime? Leadtime { get; set; }

		public virtual DateTime? LeadtimePlus { get; set; }

		public virtual DateTime? Etd { get; set; }

		public virtual DateTime? RecoatIn { get; set; }

		public virtual string IsActive { get; set; }

	}

}


