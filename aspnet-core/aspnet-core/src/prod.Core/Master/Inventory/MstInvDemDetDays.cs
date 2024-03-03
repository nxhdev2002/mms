using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{
	[Table("MstInvDemDetDays")]
	public class MstInvDemDetDays : FullAuditedEntity<long>, IEntity<long>
	{
		public const int MaxExpLength = 50;
		public const int MaxCarrieLength = 50;
		public const int MaxCombineDEMDETLength = 50;

		[StringLength(MaxExpLength)]
		public virtual string Exp { get; set; }
		[StringLength(MaxCarrieLength)]
		public virtual string Carrier { get; set; }
		[StringLength(MaxCombineDEMDETLength)]
		public virtual string CombineDEMDET { get; set; }
		public virtual int FreeDEM { get; set; }
		public virtual int FreeDET { get; set; }
		public virtual int CombineDEMDETFree { get; set; }

	}
}
