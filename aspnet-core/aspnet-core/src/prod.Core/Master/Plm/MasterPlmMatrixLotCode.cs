using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Plm
{
    [Table("MasterPlmMatrixLotCode")]
    [Index(nameof(ScreenId), Name = "IX_MasterPlmMatrixLotCode_ScreenId")]
    [Index(nameof(LotcodeGradeId), Name = "IX_MasterPlmMatrixLotCode_LotcodeGradeId")]
    public class MasterPlmMatrixLotCode : FullAuditedEntity<long>, IEntity<long>
    {
        public virtual int? ScreenId { get; set; }

        public virtual int? LotcodeGradeId { get; set; }

        public virtual int? PartId { get; set; }
    }

}
