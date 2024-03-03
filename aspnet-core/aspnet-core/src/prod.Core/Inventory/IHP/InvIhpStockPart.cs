using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IHP
{
    [Table("InvIhpStockPart")]
    public class InvIhpStockPart : FullAuditedEntity<long>, IEntity<long>
    {
        public virtual long? DrmMaterialId { get; set; }
        public virtual string PartCode { get; set; }
        public virtual int? Qty { get; set; }
        public virtual DateTime WorkingDate { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartNo5Digits { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string Model { get; set; }
        public virtual string GradeName { get; set; }
        public virtual int? UsePress { get; set; }
        public virtual int? Press { get; set; }
        public virtual int? IhpOh { get; set; }
        public virtual int? PressBroken { get; set; }
        public virtual int? Hand { get; set; }
        public virtual int? HandOh { get; set; }
        public virtual int? HandBroken { get; set; }
        public virtual int? MaterialIn { get; set; }
        public virtual int? MaterialInAddition { get; set; }
        public virtual string Shift { get; set; }
        public virtual long? PartId { get; set; }
    }
}
