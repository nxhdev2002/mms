using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwPlcSignal")]
	[Index(nameof(SignalIndex), Name = "IX_MstLgwPlcSignal_SignalIndex")]
	[Index(nameof(ProdLine), Name = "IX_MstLgwPlcSignal_ProdLine")]
	[Index(nameof(SignalCode), Name = "IX_MstLgwPlcSignal_SignalCode")]
	[Index(nameof(IsActive), Name = "IX_MstLgwPlcSignal_IsActive")]
	public class MstLgwPlcSignal : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxSignalPatternLength = 250;

		public const int MaxProdLineLength = 50;

		public const int MaxProcessLength = 50;

		public const int MaxSubProcessLength = 50;

		public const int MaxSignalCodeLength = 50;

		public const int MaxSignalDescriptionLength = 500;

		public const int MaxIsActiveLength = 1;

		public virtual int? SignalIndex { get; set; }

		[StringLength(MaxSignalPatternLength)]
		public virtual string SignalPattern { get; set; }

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[StringLength(MaxProcessLength)]
		public virtual string Process { get; set; }

		[StringLength(MaxSubProcessLength)]
		public virtual string SubProcess { get; set; }

		[StringLength(MaxSignalCodeLength)]
		public virtual string SignalCode { get; set; }

		[StringLength(MaxSignalDescriptionLength)]
		public virtual string SignalDescription { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

