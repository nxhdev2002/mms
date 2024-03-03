using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.LogW
{
  
	[Table("MstLgwModelGrade_T")]
	[Index(nameof(Model), Name = "IX_MstLgwModelGrade_T_Model")]
	[Index(nameof(Grade), Name = "IX_MstLgwModelGrade_T_Grade")]
	[Index(nameof(IsActive), Name = "IX_MstLgwModelGrade_T_IsActive")]
	public class MstLgwModelGrade_T : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxModelLength = 10;

		public const int MaxGradeLength = 10;

		public const int MaxIsActiveLength = 50;

		public const int MaxGuidLength = 128;

		[StringLength(MaxGuidLength)]
		public virtual string Guid { get; set; }

		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MaxGradeLength)]
		public virtual string Grade { get; set; }

		public virtual int? ModuleUpQty { get; set; }

		public virtual int? ModuleMkQty { get; set; }

		public virtual int? UpLeadtime { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}
}
