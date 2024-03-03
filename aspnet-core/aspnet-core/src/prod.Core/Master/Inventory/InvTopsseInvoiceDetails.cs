using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

    [Table("InvTopsseInvoiceDetails")]
    public class InvTopsseInvoiceDetails : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCaseNoLength = 8;

        public const int MaxOrderNoLength = 8;

        public const int MaxItemNoLength = 4;

        public const int MaxPartNoLength = 15;

        public const int MaxPartNameLength = 100;

        public const int MaxTariffCdLength = 3;

        public const int MaxHsCdLength = 15;


        [StringLength(MaxCaseNoLength)]
        public virtual string CaseNo { get; set; }

        [StringLength(MaxOrderNoLength)]
        public virtual string OrderNo { get; set; }

        [StringLength(MaxItemNoLength)]
        public virtual string ItemNo { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual int? PartQty { get; set; }

        public virtual decimal? UnitFob { get; set; }

        public virtual decimal? Amount { get; set; }

        [StringLength(MaxTariffCdLength)]
        public virtual string TariffCd { get; set; }

        [StringLength(MaxHsCdLength)]
        public virtual string HsCd { get; set; }

        public virtual long? InvoiceId { get; set; }
    }

}


