using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogA
{
    [Table("MstLgaEkbPartListGrade")]
    [Index(nameof(PartNo), Name = "IX_MstLgaEkbPartListGrade_PartNo")]
    [Index(nameof(PartNoNormanlized), Name = "IX_MstLgaEkbPartListGrade_PartNoNormanlized")]
    [Index(nameof(BackNo), Name = "IX_MstLgaEkbPartListGrade_BackNo")]
    [Index(nameof(PartListId), Name = "IX_MstLgaEkbPartListGrade_PartListId")]
    [Index(nameof(ProdLine), Name = "IX_MstLgaEkbPartListGrade_ProdLine")]
    [Index(nameof(ProcessId), Name = "IX_MstLgaEkbPartListGrade_ProcessId")]
    [Index(nameof(IsActive), Name = "IX_MstLgaEkbPartListGrade_IsActive")]
    public class MstLgaEkbPartListGrade : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 50;

        public const int MaxPartNoNormanlizedLength = 50;

        public const int MaxPartNameLength = 200;

        public const int MaxBackNoLength = 50;

        public const int MaxProdLineLength = 50;

        public const int MaxSupplierNoLength = 50;

        public const int MaxModelLength = 50;

        public const int MaxProcessCodeLength = 50;

        public const int MaxGradeLength = 50;

        public const int MaxModuleLength = 50;

        public const int MaxPcAddressLength = 50;

        public const int MaxSpsAddressLength = 50;

        public const int MaxRemarkLength = 500;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNoNormanlizedLength)]
        public virtual string PartNoNormanlized { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [Column(TypeName = "bigint")]
        public virtual long? PartListId { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        public virtual long? ProcessId { get; set; }

        [StringLength(MaxProcessCodeLength)]
        public virtual string ProcessCode { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual int? UsageQty { get; set; }

        public virtual int? BoxQty { get; set; }

        [StringLength(MaxModuleLength)]
        public virtual string Module { get; set; }

        [StringLength(MaxPcAddressLength)]
        public virtual string PcAddress { get; set; }

        public virtual int? PcSorting { get; set; }

        [StringLength(MaxSpsAddressLength)]
        public virtual string SpsAddress { get; set; }

        public virtual int? SpsSorting { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
