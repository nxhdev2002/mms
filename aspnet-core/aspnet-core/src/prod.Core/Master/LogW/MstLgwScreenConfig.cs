using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwScreenConfig")]
	[Index(nameof(IsActive), Name = "IX_MstLgwScreenConfig_IsActive")]
	public class MstLgwScreenConfig : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxUnpackDoneColorLength = 50;

		public const int MaxNeedToUnpackColorLength = 50;

		public const int MaxConfirmFlagColorLength = 50;

		public const int MaxConfirmFlagSoundLength = 200;

		public const int MaxDelayUnpackColorLength = 50;

		public const int MaxDelayUnpackSoundLength = 200;

		public const int MaxCallLeaderColorLength = 50;

		public const int MaxCallLeaderSoundLength = 200;

		public const int MaxBeforeTacktimeColorLength = 50;

		public const int MaxBeforeTacktimeSoundLength = 200;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxUnpackDoneColorLength)]
		public virtual string UnpackDoneColor { get; set; }

		[StringLength(MaxNeedToUnpackColorLength)]
		public virtual string NeedToUnpackColor { get; set; }

		public virtual bool NeedToUnpackFlash { get; set; }

		[StringLength(MaxConfirmFlagColorLength)]
		public virtual string ConfirmFlagColor { get; set; }

		[StringLength(MaxConfirmFlagSoundLength)]
		public virtual string ConfirmFlagSound { get; set; }

		public virtual int? ConfirmFlagPlaytime { get; set; }
		
		[Column(TypeName = "bit")]
		public virtual bool ConfirmFlagFlash { get; set; }

		[StringLength(MaxDelayUnpackColorLength)]
		public virtual string DelayUnpackColor { get; set; }

		[StringLength(MaxDelayUnpackSoundLength)]
		public virtual string DelayUnpackSound { get; set; }

		public virtual int? DelayUnpackPlaytime { get; set; }

		[Column(TypeName = "bit")]
		public virtual bool DelayUnpackFlash { get; set; }

		[StringLength(MaxCallLeaderColorLength)]
		public virtual string CallLeaderColor { get; set; }

		[StringLength(MaxCallLeaderSoundLength)]
		public virtual string CallLeaderSound { get; set; }

		public virtual int? CallLeaderPlaytime { get; set; }

		[Column(TypeName = "bit")]
		public virtual bool CallLeaderFlash { get; set; }

		public virtual int? TotalColumnOldShift { get; set; }

		public virtual int? TotalColumnSeqA1 { get; set; }

		public virtual int? TotalColumnSeqA2 { get; set; }

		[StringLength(MaxBeforeTacktimeColorLength)]
		public virtual string BeforeTacktimeColor { get; set; }

		[StringLength(MaxBeforeTacktimeSoundLength)]
		public virtual string BeforeTacktimeSound { get; set; }

		public virtual int? BeforeTacktimePlaytime { get; set; }

		[Column(TypeName = "bit")]
		public virtual bool BeforeTacktimeFlash { get; set; }

		public virtual int? TackCaseA1 { get; set; }

		public virtual int? TackCaseA2 { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

