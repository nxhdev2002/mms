using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace prod.Inventory.CKD
{
    [Table("InvCkdShipmentDetailsFirm")]
    public class InvCkdShippingScheduleDetailsFirm : FullAuditedEntity<long>, IEntity<long>
    {
        public virtual long? ShipmentHeaderId { get; set; } 

        [StringLength(20)]
        public virtual string Status { get; set; }
        [StringLength(1)]
        public virtual string Segment { get; set; }

        [StringLength(3)]
        public virtual string Seller { get; set; }

        [StringLength(3)]
        public virtual string Buyer { get; set; }

        [StringLength(6)]
        public virtual string ShippingMonth { get; set; }

        [StringLength(1)]
        public virtual string EkanbanFlag { get; set; }

        [StringLength(3)]
        public virtual string PortOfLoading { get; set; }

        [StringLength(50)]
        public virtual string PortOfDischarge { get; set; }

        [StringLength(8)]
        public virtual string VesselEtd1st { get; set; }

        [StringLength(30)]
        public virtual string VesselName1st { get; set; }

        [StringLength(10)]
        public virtual string VesselNo1st { get; set; }

        public virtual int? RevisionNo { get; set; }

        [StringLength(4)]
        public virtual string ExporterCode { get; set; }

        [StringLength(4)]
        public virtual string ImporterCode { get; set; }

        [StringLength(21)]
        public virtual string OrderControlNo { get; set; }

        [StringLength(6)]
        public virtual string RenbanNo { get; set; }

        public virtual int? ContainerSize { get; set; }

        [StringLength(6)]
        public virtual string CarFamilyCode { get; set; }

        [StringLength(6)]
        public virtual string PackingMonth { get; set; }

        [StringLength(20)]
        public virtual string ModelCode { get; set; }

        [StringLength(6)]
        public virtual string LotNo { get; set; }

        [StringLength(4)]
        public virtual string CaseNo { get; set; }

        [StringLength(6)]
        public virtual string ModuleNo { get; set; }

        [StringLength(7)]
        public virtual string FormDRenban { get; set; }

        public virtual int? M3OfModule { get; set; }

        public virtual int? GrossWeightOfModule { get; set; }

        [StringLength(1)]
        public virtual string ReExportCode { get; set; }

        [StringLength(2)]
        public virtual string SsNo { get; set; }

        [StringLength(1)]
        public virtual string LineCode { get; set; }

        [StringLength(12)]
        public virtual string PartNo { get; set; }

        [StringLength(2)]
        public virtual string LotCode { get; set; }

        [StringLength(4)]
        public virtual string ExteriorColorCode { get; set; }

        [StringLength(4)]
        public virtual string InteriorColorCode { get; set; }

        public virtual int? OrderLot { get; set; }

        public virtual int? ScheduleQty { get; set; }

        public virtual int? HaisenIndex { get; set; }

        [StringLength(3)]
        public virtual string ValuationTypeFrom { get; set; }

        [StringLength(8)]
        public virtual string PackingDate { get; set; }

        [StringLength(1)]
        public virtual string IsActive { get; set; }
    }
}
