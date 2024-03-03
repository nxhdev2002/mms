using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwModelGrade")]
	[Index(nameof(Model), Name = "IX_MstLgwModelGrade_Model")]
	[Index(nameof(Grade), Name = "IX_MstLgwModelGrade_Grade")]
	[Index(nameof(IsActive), Name = "IX_MstLgwModelGrade_IsActive")]
	public class MstLgwModelGrade : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxModelLength = 10;

		public const int MaxGradeLength = 10;

		public const int MaxIsActiveLength = 50;

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

