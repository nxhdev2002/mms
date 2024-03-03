using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Cmm
{
	[Table("MstCmmGradeColor")]
	public class MstCmmGradeColor : FullAuditedEntity<long>, IEntity<long>
	{
		public virtual int? GradeId { get; set; }

        public virtual int? ColorId { get; set; }

    }
}


