using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

    [Table("MstInvCustomsPort")]
    [Index(nameof(Code), Name = "IX_MstInvCustomsPort_Code")]
    [Index(nameof(Name), Name = "IX_MstInvCustomsPort_Name")]
    [Index(nameof(IsActive), Name = "IX_MstInvCustomsPort_IsActive")]
    public class MstInvCustomsPort : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 12;

        public const int MaxNameLength = 50;

        public const int MaxAccountNumberLength = 30;

        public const int MaxBankNameLength = 100;

        public const int MaxVendorNumberLength = 30;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxAccountNumberLength)]
        public virtual string AccountNumber { get; set; }

        [StringLength(MaxBankNameLength)]
        public virtual string BankName { get; set; }

        [StringLength(MaxVendorNumberLength)]
        public virtual string VendorNumber { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

