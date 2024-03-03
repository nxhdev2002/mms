using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogA.Bar
{

    [Table("LgaBarScanInfo")]
    [Index(nameof(UserId), Name = "IX_LgaBarScanInfo_UserId")]
    [Index(nameof(UserName), Name = "IX_LgaBarScanInfo_UserName")]
    [Index(nameof(ScanType), Name = "IX_LgaBarScanInfo_ScanType")]
    [Index(nameof(ProdLine), Name = "IX_LgaBarScanInfo_ProdLine")]
    [Index(nameof(RefId), Name = "IX_LgaBarScanInfo_RefId")]
    [Index(nameof(IsActive), Name = "IX_LgaBarScanInfo_IsActive")]
    public class LgaBarScanInfo : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxUserIdLength = 50;

        public const int MaxUserNameLength = 50;

        public const int MaxScanValueLength = 500;

        public const int MaxScanPartNoLength = 50;

        public const int MaxScanBackNoLength = 50;

        public const int MaxScanTypeLength = 50;

        public const int MaxProdLineLength = 50;

        public const int MaxStatusLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxUserIdLength)]
        public virtual string UserId { get; set; }

        [StringLength(MaxUserNameLength)]
        public virtual string UserName { get; set; }

        [StringLength(MaxScanValueLength)]
        public virtual string ScanValue { get; set; }

        [StringLength(MaxScanPartNoLength)]
        public virtual string ScanPartNo { get; set; }

        [StringLength(MaxScanBackNoLength)]
        public virtual string ScanBackNo { get; set; }

        [StringLength(MaxScanTypeLength)]
        public virtual string ScanType { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? ScanDatetime { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        public virtual long? RefId { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}