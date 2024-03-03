using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwScreenConfigDto : EntityDto<long?>
	{

		public virtual string UnpackDoneColor { get; set; }

		public virtual string NeedToUnpackColor { get; set; }

		public virtual bool NeedToUnpackFlash { get; set; }

		public virtual string ConfirmFlagColor { get; set; }

		public virtual string ConfirmFlagSound { get; set; }

		public virtual int? ConfirmFlagPlaytime { get; set; }

		public virtual bool ConfirmFlagFlash { get; set; }

		public virtual string DelayUnpackColor { get; set; }

		public virtual string DelayUnpackSound { get; set; }

		public virtual int? DelayUnpackPlaytime { get; set; }

		public virtual bool DelayUnpackFlash { get; set; }

		public virtual string CallLeaderColor { get; set; }

		public virtual string CallLeaderSound { get; set; }

		public virtual int? CallLeaderPlaytime { get; set; }

		public virtual bool CallLeaderFlash { get; set; }

		public virtual int? TotalColumnOldShift { get; set; }

		public virtual int? TotalColumnSeqA1 { get; set; }

		public virtual int? TotalColumnSeqA2 { get; set; }

		public virtual string BeforeTacktimeColor { get; set; }

		public virtual string BeforeTacktimeSound { get; set; }

		public virtual int? BeforeTacktimePlaytime { get; set; }

		public virtual bool BeforeTacktimeFlash { get; set; }

		public virtual int? TackCaseA1 { get; set; }

		public virtual int? TackCaseA2 { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgwScreenConfigDto : EntityDto<long?>
	{

		[StringLength(MstLgwScreenConfigConsts.MaxUnpackDoneColorLength)]
		public virtual string UnpackDoneColor { get; set; }

		[StringLength(MstLgwScreenConfigConsts.MaxNeedToUnpackColorLength)]
		public virtual string NeedToUnpackColor { get; set; }

		public virtual bool NeedToUnpackFlash { get; set; }

		[StringLength(MstLgwScreenConfigConsts.MaxConfirmFlagColorLength)]
		public virtual string ConfirmFlagColor { get; set; }

		[StringLength(MstLgwScreenConfigConsts.MaxConfirmFlagSoundLength)]
		public virtual string ConfirmFlagSound { get; set; }

		public virtual int? ConfirmFlagPlaytime { get; set; }

		public virtual bool ConfirmFlagFlash { get; set; }

		[StringLength(MstLgwScreenConfigConsts.MaxDelayUnpackColorLength)]
		public virtual string DelayUnpackColor { get; set; }

		[StringLength(MstLgwScreenConfigConsts.MaxDelayUnpackSoundLength)]
		public virtual string DelayUnpackSound { get; set; }

		public virtual int? DelayUnpackPlaytime { get; set; }

		public virtual bool DelayUnpackFlash { get; set; }

		[StringLength(MstLgwScreenConfigConsts.MaxCallLeaderColorLength)]
		public virtual string CallLeaderColor { get; set; }

		[StringLength(MstLgwScreenConfigConsts.MaxCallLeaderSoundLength)]
		public virtual string CallLeaderSound { get; set; }

		public virtual int? CallLeaderPlaytime { get; set; }

		public virtual bool CallLeaderFlash { get; set; }

		public virtual int? TotalColumnOldShift { get; set; }

		public virtual int? TotalColumnSeqA1 { get; set; }

		public virtual int? TotalColumnSeqA2 { get; set; }

		[StringLength(MstLgwScreenConfigConsts.MaxBeforeTacktimeColorLength)]
		public virtual string BeforeTacktimeColor { get; set; }

		[StringLength(MstLgwScreenConfigConsts.MaxBeforeTacktimeSoundLength)]
		public virtual string BeforeTacktimeSound { get; set; }

		public virtual int? BeforeTacktimePlaytime { get; set; }

		public virtual bool BeforeTacktimeFlash { get; set; }

		public virtual int? TackCaseA1 { get; set; }

		public virtual int? TackCaseA2 { get; set; }

		[StringLength(MstLgwScreenConfigConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstLgwScreenConfigInput : PagedAndSortedResultRequestDto
	{

		public virtual string UnpackDoneColor { get; set; }

		public virtual string NeedToUnpackColor { get; set; }

		public virtual string ConfirmFlagColor { get; set; }

		public virtual string ConfirmFlagSound { get; set; }

		public virtual int? ConfirmFlagPlaytime { get; set; }

		public virtual string DelayUnpackColor { get; set; }

		public virtual string DelayUnpackSound { get; set; }

		public virtual int? DelayUnpackPlaytime { get; set; }

		public virtual string CallLeaderColor { get; set; }

		public virtual string CallLeaderSound { get; set; }

		public virtual int? CallLeaderPlaytime { get; set; }

		public virtual int? TotalColumnOldShift { get; set; }

		public virtual int? TotalColumnSeqA1 { get; set; }

		public virtual int? TotalColumnSeqA2 { get; set; }

		public virtual string BeforeTacktimeColor { get; set; }

		public virtual string BeforeTacktimeSound { get; set; }

		public virtual int? BeforeTacktimePlaytime { get; set; }

		public virtual int? TackCaseA1 { get; set; }

		public virtual int? TackCaseA2 { get; set; }

		public virtual string IsActive { get; set; }

	}

}


