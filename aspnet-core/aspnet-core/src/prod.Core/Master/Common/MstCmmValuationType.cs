using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Common
{

    [Table("MstCmmValuationType")]
    [Index(nameof(Code), Name = "IX_MstCmmValuationType_Code")]
    [Index(nameof(Product), Name = "IX_MstCmmValuationType_Product")]
    [Index(nameof(MaterialTypeId), Name = "IX_MstCmmValuationType_MaterialTypeId")]
    [Index(nameof(MaterialType), Name = "IX_MstCmmValuationType_MaterialType")]
    [Index(nameof(IsActive), Name = "IX_MstCmmValuationType_IsActive")]
    public class MstCmmValuationType : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 4;

        public const int MaxNameLength = 25;

        public const int MaxProductLength = 25;

        public const int MaxMaterialTypeIdLength = 25;

        public const int MaxMaterialTypeLength = 200;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxProductLength)]
        public virtual string Product { get; set; }

        [StringLength(MaxMaterialTypeIdLength)]
        public virtual string MaterialTypeId { get; set; }

        [StringLength(MaxMaterialTypeLength)]
        public virtual string MaterialType { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

