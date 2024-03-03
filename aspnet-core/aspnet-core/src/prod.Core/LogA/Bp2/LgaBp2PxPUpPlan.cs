using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prod.LogA.Bp2
{
    [Table("LgaBp2PxPUpPlan")]
    [Index(nameof(ProdLine), Name = "IX_LgaBp2PxPUpPlan_Shift")]
    [Index(nameof(WorkingDate), Name = "IX_LgaBp2PxPUpPlan_WorkingDate")]
    [Index(nameof(Status), Name = "IX_LgaBp2PxPUpPlan_Status")]
    [Index(nameof(IsActive), Name = "IX_LgaBp2PxPUpPlan_IsActive")]
    public class LgaBp2PxPUpPlan : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxProdLineLength = 50;
        public const int MaxUnpackingTimeLength = 50;
        public const int MaxCaseNoLength = 50;
        public const int MaxSupplierNoLength = 50;
        public const int MaxModelLength = 50;
        public const int MaxShiftLength = 50;
        public const int MaxUpTableLength = 20;
        public const int MaxUnpackingByLength = 50;
        public const int MaxRemarksLength = 50;
        public const int MaxIsNewPartLength = 1;
        public const int MaxIsActiveLength = 1;



        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        public virtual int? NoOfALineIn { get; set; }

        [StringLength(MaxUnpackingTimeLength)]
        public virtual string UnpackingTime { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? UnpackingDate { get; set; }

        [StringLength(MaxCaseNoLength)]
        public virtual string CaseNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        public virtual int? TotalNoInShift { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? UnpackingDatetime { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }

        [StringLength(MaxUpTableLength)]
        public virtual string UpTable { get; set; }

        public virtual int? UpLt { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? UnpackingStartDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? UnpackingFinishDatetime { get; set; }

        public virtual int? UnpackingSecond { get; set; }

        [StringLength(MaxUnpackingByLength)]
        public virtual string UnpackingBy { get; set; }

        public virtual int? DelaySecond { get; set; }

        public virtual int? TimeOffSecond { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? StartPauseTime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? EndPauseTime { get; set; }


        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? DelayConfirmFlag { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? FinishConfirmFlag { get; set; }

        public virtual int? DelayConfirmSecond { get; set; }

        public virtual int? TimeOffConfirmSecond { get; set; }

        public virtual string WhLocation { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        [StringLength(MaxRemarksLength)]
        public virtual string Remarks { get; set; }

        [StringLength(MaxIsNewPartLength)]
        public virtual string IsNewPart { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
        public virtual int? Status { get; set; }
    }
}
