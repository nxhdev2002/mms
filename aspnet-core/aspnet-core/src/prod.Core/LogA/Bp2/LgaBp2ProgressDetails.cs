using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Castle.MicroKernel.SubSystems.Conversion;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.LogA.Bp2
{
    [Table("LgaBp2ProgressDetails")]   
    [Index(nameof(ProdLine), Name = "IX_LgaBp2ProgressDetails_Shift")]
    [Index(nameof(WorkingDate), Name = "IX_LgaBp2ProgressDetails_WorkingDate")]
    [Index(nameof(Status), Name = "IX_LgaBp2ProgressDetails_Status")]
    [Index(nameof(IsActive), Name = "IX_LgaBp2ProgressDetails_IsActive")]
    public class LgaBp2ProgressDetails : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxProdLineLength = 50;

        public const int MaxShiftLength = 50;


        public const int MaxPartNoLength = 50;
        public const int MaxShortNameLength = 50;
        public const int MaxAddressLength = 50;
        public const int MaxTypeLength = 50;
        public const int MaxBodyNoLength = 50;
        public const int MaxLotNoLength = 50;
        public const int MaxGradeLength = 50;
        public const int MaxModelLength = 50;
        public const int MaxBodyNo2Length = 50;
        public const int MaxLotNo2Length = 50;
        public const int MaxGrade2Length = 50;
        public const int MaxModel2Length = 50;
        public const int MaxAddress2Length = 50;
        public const int MaxStatusLength = 50;
        public const int MaxIsActiveLength = 1;




        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }
        public virtual int? NoInShift { get; set; }

        public virtual int? ProgressId { get; set; }
        public virtual int? EcarId { get; set; }
        public virtual int? PartListId { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxShortNameLength)]
        public virtual string ShortName { get; set; }

        [StringLength(MaxAddressLength)]
        public virtual string Address { get; set; }

        [StringLength(MaxTypeLength)]
        public virtual string Type { get; set; }

        public virtual int? Sorting { get; set; }

        [StringLength(MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxBodyNo2Length)]
        public virtual string BodyNo2 { get; set; }

        [StringLength(MaxLotNo2Length)]
        public virtual string LotNo2 { get; set; }

        public virtual int? NoInLot2 { get; set; }

        [StringLength(MaxGrade2Length)]
        public virtual string Grade2 { get; set; }

        [StringLength(MaxModel2Length)]
        public virtual string Model2 { get; set; }

        [StringLength(MaxAddress2Length)]
        public virtual string Address2 { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? NewtaktDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? StartDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? FinishDatetime { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
