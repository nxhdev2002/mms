using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Asy.Plm
{
    [Table("AsyPlmAssemblyData")]
    [Index(nameof(GroupId), Name = "IX_AsyPlmAssemblyData_GroupId")]
    [Index(nameof(Line), Name = "IX_AsyPlmAssemblyData_Line")]
    [Index(nameof(BodyNo), Name = "IX_AsyPlmAssemblyData_BodyNo")]
    [Index(nameof(SeqNo), Name = "IX_AsyPlmAssemblyData_SeqNo")]
    [Index(nameof(NoInLot), Name = "IX_AsyPlmAssemblyData_NoInLot")]
    [Index(nameof(Color), Name = "IX_AsyPlmAssemblyData_Color")]
    [Index(nameof(TackTime), Name = "IX_AsyPlmAssemblyData_TackTime")]
    [Index(nameof(NoInDate), Name = "IX_AsyPlmAssemblyData_NoInDate")]
    public class AsyPlmAssemblyData : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxGroupIdLength = 500;

        public const int MaxLineLength = 50;

        public const int MaxBodyNoLength = 500;

        public const int MaxSeqNoLength = 50;

        public const int MaxNoInLotLength = 500;

        public const int MaxColorLength = 50;

        public const int MaxTackTimeLength = 500;

        public const int MaxNoInDateLength = 50;


        [StringLength(MaxGroupIdLength)]
        public virtual string GroupId { get; set; }

        [StringLength(MaxLineLength)]
        public virtual string Line { get; set; }

        public virtual int? Process { get; set; }

        public virtual int? Model { get; set; }

        [StringLength(MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        [StringLength(MaxSeqNoLength)]
        public virtual string SeqNo { get; set; }

        public virtual int? Grade { get; set; }

        public virtual int? LotNo { get; set; }

        [StringLength(MaxNoInLotLength)]
        public virtual string NoInLot { get; set; }

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        public virtual int? WorkingDate { get; set; }

        public virtual int? CreateDate { get; set; }

        [StringLength(MaxTackTimeLength)]
        public virtual string TackTime { get; set; }

        [StringLength(MaxNoInDateLength)]
        public virtual string NoInDate { get; set; }

        public virtual int? LotNoIndex { get; set; }
    }

}
