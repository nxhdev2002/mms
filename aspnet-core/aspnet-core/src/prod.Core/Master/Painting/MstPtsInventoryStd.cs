using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Painting
{

    [Table("MstPtsInventoryStd")]
    [Index(nameof(Model), Name = "IX_MstPtsInventoryStd_Model")]
    [Index(nameof(IsActive), Name = "IX_MstPtsInventoryStd_IsActive")]
    public class MstPtsInventoryStd : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxModelLength = 1;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        public virtual int? InventoryStd { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}