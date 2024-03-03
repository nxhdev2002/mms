using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Welding.Andon
{

	[Table("WldAdoWeldingSignal")]
	[Index(nameof(ProdLine), Name = "IX_WldAdoWeldingSignal_ProdLine")]
	[Index(nameof(Process), Name = "IX_WldAdoWeldingSignal_Process")]
	[Index(nameof(SignalDate), Name = "IX_WldAdoWeldingSignal_SignalDate")]
	public class WldAdoWeldingSignal : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxProdLineLength = 10;

		public const int MaxProcessLength = 20;

		public const int MaxSignalTypeLength = 20;

		public const int MaxSignalByLength = 20;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[StringLength(MaxProcessLength)]
		public virtual string Process { get; set; }

		[StringLength(MaxSignalTypeLength)]
		public virtual string SignalType { get; set; }

		[StringLength(MaxSignalByLength)]
		public virtual string SignalBy { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? SignalDate { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}
