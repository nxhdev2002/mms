using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace prod.Master.Inventory
{

    [Table("MstInvCkdRentalWarehouse")]
    public class MstInvCkdRentalWarehouse : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 3;

        public const int MaxNameLength = 50;

        public const int MaxIsActiveLength = 1;

		[StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }

	}

}

