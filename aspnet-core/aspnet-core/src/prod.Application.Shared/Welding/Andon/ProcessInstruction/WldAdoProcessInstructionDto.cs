using Abp.Application.Services.Dto;
using System; 
namespace prod.Welding.Andon.Dto
{


	public class GetProcessInstructionDataOutput : EntityDto<long?>
	{
		public virtual string CurrentLotNo { get; set; } 
		public virtual string NextLotNo { get; set; } 
		public virtual string CurrentBodyNo { get; set; }
		public virtual string NextBodyNo { get; set; }

		public virtual DateTime? StartTime { get; set; }
		public virtual DateTime? CurrentTime { get; set; }
		public virtual int? NoOfCallPart { get; set; }
		public virtual int? ChangeTipPink { get; set; } 
		public virtual int? ChangeTipOrange { get; set; }
		public virtual int? ChangeTipYellow { get; set; }
		public virtual int? ChangeTipGreen { get; set; }
		public virtual int? ChangeTipWhite { get; set; }
		public virtual double TatkTime { get; set; }
		public virtual int? PlanNum { get; set; }
		public virtual int? ActualNum { get; set; }
		public virtual int? DelayNum { get; set; }
		public virtual int? Eff { get; set; } 

	}


}

