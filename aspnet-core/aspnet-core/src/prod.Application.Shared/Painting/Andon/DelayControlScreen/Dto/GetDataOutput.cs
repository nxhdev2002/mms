using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class GetDataOutput : EntityDto<long?>
	{
		public virtual int? RowNumber { get; set; }

		public virtual int? Id { get; set; }
		public virtual string BodyNo { get; set; }

		public virtual string Color { get; set; }

		public virtual DateTime? LeadtimePlus { get; set; }

		public virtual string TotalDelay { get; set; }

		public virtual int? TotalDelayAct { get; set; }

		public virtual DateTime? Etd { get; set; }

		public virtual DateTime? AInPlanDate { get; set; }

		public virtual string Mode { get; set; }

		public virtual string Location { get; set; }

		public virtual double DifRepairIn { get; set; }

		public virtual double DiffStartRepair { get; set; }

		public virtual double DiffRecoat { get; set; }

		public virtual double DiffNow { get; set; }

		public virtual double DiffETD { get; set; }

		public virtual double RemainingTime { get; set; }

		public virtual DateTime? RepairIn { get; set; }

		public virtual DateTime? StartRepair { get; set; }

		public virtual DateTime? RecoatIn { get; set; }

		public virtual DateTime? Leadtime { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string IsActive { get; set; }

	}


}


