using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Pup.Dto
{

	public class LgwPupPxPUpPlanOutputDto
	{

		public virtual string ProdLine { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		public virtual string Shift { get; set; }

		public virtual int? NoInShift { get; set; }

		public virtual int? SeqLineIn { get; set; }

		public virtual string CaseNo { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string UpTable { get; set; }

		public virtual string IsNoPxpData { get; set; }

		public virtual DateTime? UnpackingStartDatetime { get; set; }

		public virtual DateTime? UnpackingFinishDatetime { get; set; }

		public virtual string UnpackingTime { get; set; }

		public virtual DateTime? UnpackingDate { get; set; }

		public virtual DateTime? UnpackingDatetime { get; set; }

		public virtual int? UpLt { get; set; }

		public virtual string Status { get; set; }

		public virtual int? DelaySecond { get; set; }

		public virtual DateTime? DelayConfirmFlag { get; set; }

		public virtual string WhLocation { get; set; }

		public virtual DateTime? InvoiceDate { get; set; }

		public virtual string Remarks { get; set; }

		public virtual string IsActive { get; set; }

		public virtual string IsFinished { get; set; }

		public virtual string IsStarted { get; set; }

		public virtual string IsDelayed { get; set; }

		public virtual string IsCalling { get; set; }

		public virtual string IsPreviousShiftDelay { get; set; }

		public virtual int? TotalBlock { get; set; }

		public virtual int? PlanVol { get; set; }

		public virtual int? ActualVol { get; set; }

		public virtual int? LeadTime { get; set; }

		public virtual int? TaktTime { get; set; }

	}

}


