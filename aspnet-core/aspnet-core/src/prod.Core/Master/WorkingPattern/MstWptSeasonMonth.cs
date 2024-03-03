using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.WorkingPattern
{

	[Table("MstWptSeasonMonth")]
	[Index(nameof(SeasonMonth), Name = "IX_MstWptSeasonMonth_SeasonMonth")]
	public class MstWptSeasonMonth : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxSeasonTypeLength = 10;

		public const int MaxIsActiveLength = 1;

		public virtual DateTime SeasonMonth { get; set; }

		[StringLength(MaxSeasonTypeLength)]
		public virtual string SeasonType { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}
