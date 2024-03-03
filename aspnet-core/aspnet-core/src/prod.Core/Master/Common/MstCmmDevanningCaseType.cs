using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Common
{

    [Table("MstCmmDevanningCaseType")]
    [Index(nameof(CaseNo), Name = "IX_MstCmmDevanningCaseType_CaseNo")]
    [Index(nameof(IsActive), Name = "IX_MstCmmDevanningCaseType_IsActive")]
    public class MstCmmDevanningCaseType : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCaseNoLength = 5;

        public const int MaxCarFamilyCodeLength = 4;

        public const int MaxShoptypeCodeLength = 2;

        public const int MaxSupplierNoLength = 10;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCaseNoLength)]
        public virtual string CaseNo { get; set; }

        [StringLength(MaxCarFamilyCodeLength)]
        public virtual string CarFamilyCode { get; set; }

        [StringLength(MaxShoptypeCodeLength)]
        public virtual string ShoptypeCode { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

