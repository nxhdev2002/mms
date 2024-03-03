using Abp.Application.Services.Dto;
using System; 
namespace prod.LogA.Bp2
{

	public class LgaBp2ProgressScreenDto : EntityDto<long?>
	{
         

	}

	public class LgaBp2ProgressMonitorScreenDto : EntityDto<long?>
	{
		 
		public virtual string Title { get; set; }
		public virtual string ProdLine { get; set; }
		public virtual DateTime? WorkingDate { get; set; }
		public virtual string Shift { get; set; }
		public virtual int? NoInShift { get; set; }
		public virtual string EcarName { get; set; }
		public virtual string Code { get; set; }
		public virtual string ProcessName { get; set; }
		public virtual int? Sorting { get; set; }
		public virtual int? TotalCycle { get; set; } //Tong so cycle chia tren man hinh Monitor
		public virtual int? SequenceNo { get; set; }
		public virtual int? NumberNo { get; set; }
		public virtual int? Efficiency { get; set; }
		public virtual DateTime? StartDatetime { get; set; }
		public virtual DateTime? FinishDatetime { get; set; }
		public virtual int? DelaySecond { get; set; }
		public virtual string IsDelay { get; set; }
		public virtual int? TaktTime { get; set; }
		public virtual string IsPaused { get; set; }
		public virtual string IsStoped { get; set; }
		public virtual string Status { get; set; }

	}


	public class LgaBp2ProgressScreenConfigOutputDto
	{

		public virtual int? EcarId { get; set; }
		public virtual int? ProgressId { get; set; } 
		public virtual string Title { get; set; } 
		public virtual string ProdLine { get; set; }
		public virtual DateTime? WorkingDate { get; set; }
		public virtual string Shift { get; set; } 
		public virtual int? NoInShift { get; set; }
		public virtual string EcarName { get; set; }
		public virtual string Code { get; set; }
		public virtual string ProcessName { get; set; }
		public virtual int? Sorting { get; set; } 
		public virtual int? ActualVolCount { get; set; }
		public virtual int? PlanVolCount { get; set; }
		public virtual int? EcarCount { get; set; }
		public virtual int? DelaySecond { get; set; }
		public virtual int? ActualTrim { get; set; }
		public virtual int? PlanTrim { get; set; } 
		public virtual DateTime? StartDatetime { get; set; }
		public virtual DateTime? FinishDatetime { get; set; } 
		public virtual int? TotalTaktTime { get; set; }
		public virtual int? ProcessTaktTime { get; set; }
		public virtual int? ProcessActualTime { get; set; }
		public virtual DateTime? NewTaktStartTime { get; set; }
		public virtual string IsDelayStart { get; set; }    // Hien thi tam giac mau do neu truong nay la 'Y', tam giac mau vang neu la 'N', khong hien thi tam giac neu la NULL
		public virtual string IsDelayStart1 { get; set; }
		public virtual string ScreenStatus { get; set; }
		public virtual string Status { get; set; }  //STOPED, PAUSED(theo tin hieu cua Trim A1 hoac theo trang thai cua Trip: NULL / NEWTAKT / STARTED/ FINISHED / DELAYED / exclude COMPLETED
		public virtual int? NTSignalCount { get; set; } //tru tin hieu dau tien
		public virtual int? DelayStartSecond { get; set; }
		public virtual int? NewTaktOffset { get; set; }
		public virtual int? RemainingTime { get; set; } //neu thoi gian nay < -50 thi se tinh la delay 
		public virtual int? TotalCycle { get; set; } //Tong so cycle chia tren man hinh Monitor

	}
}

