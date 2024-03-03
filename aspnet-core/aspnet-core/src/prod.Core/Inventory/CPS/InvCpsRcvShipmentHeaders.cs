using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CPS
{

    [Table("InvCpsRcvShipmentHeaders")]
    public class InvCpsRcvShipmentHeaders : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxReceiptSourceCodeLength = 25;

        public const int MaxReceiptNumLength = 30;

        public const int MaxBillOfLadingLength = 25;

        public const int MaxCommentsLength = 4000;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxReceiptSourceCodeLength)]
        public virtual string ReceiptSourceCode { get; set; }

        public virtual long? VendorId { get; set; }

        [StringLength(MaxReceiptNumLength)]
        public virtual string ReceiptNum { get; set; }

        [StringLength(MaxBillOfLadingLength)]
        public virtual string BillOfLading { get; set; }

        public virtual long? EmployeeId { get; set; }

        [StringLength(MaxCommentsLength)]
        public virtual string Comments { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}