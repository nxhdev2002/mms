using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{
    [Table("MstInvSupplierList")]
    [Index(nameof(SupplierNo), Name = "IX_MstInvSupplierList_SupplierNo")]
    public class MstInvSupplierList : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxSupplierNoLength = 10;

        public const int MaxSupplierNameLength = 50;

        public const int MaxRemarksLength = 1000;

        public const int MaxSupplierTypeLength = 10;

        public const int MaxSupplierNameVnLength = 50;

        public const int MaxExporterLength = 200;

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }

        [StringLength(MaxRemarksLength)]
        public virtual string Remarks { get; set; }

        [StringLength(MaxSupplierTypeLength)]
        public virtual string SupplierType { get; set; }

        [StringLength(MaxSupplierNameVnLength)]
        public virtual string SupplierNameVn { get; set; }

        [StringLength(MaxExporterLength)]
        public virtual string Exporter { get; set; }
    }

}