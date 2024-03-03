using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdModuleCase")]
    public class InvCkdModuleCase : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxModuleNoLength = 12;

        public const int MaxCaseNoLength = 12;

        public const int MaxContainerNoLength = 15;

        public const int MaxRenbanLength = 6;

        public const int MaxLotNoLength = 10;

        public const int MaxSupplierNoLength = 10;

        public const int MaxDummyLength = 10;

        public const int MaxDgflagLength = 5;

        public const int MaxPackingTypeLength = 3;

        public const int MaxRrTypeLength = 2;

        public const int MaxInnerMaterialType1Length = 2;

        public const int MaxInnerMaterialType2Length = 2;

        public const int MaxInnerMaterialType3Length = 2;

        public const int MaxInnerMaterialType4Length = 2;

        public const int MaxInnerMaterialType5Length = 2;

        public const int MaxInnerMaterialType6Length = 2;

        public const int MaxInnerMaterialType7Length = 2;

        public const int MaxInnerMaterialType8Length = 2;

        public const int MaxInnerMaterialType9Length = 2;

        public const int MaxInnerMaterialType10Length = 2;

        public const int MaxInnerMaterialType11Length = 2;

        public const int MaxInnerMaterialType12Length = 2;

        public const int MaxIsActiveLength = 1;

        public const int MaxModuleCaseNoLength = 12;

        public const int MaxInvoiceNoLength = 20;

        public const int MaxStorageLocationCodeLength = 12;

        public const int MaxLocationByLength = 50;


        [StringLength(MaxModuleNoLength)]
        public virtual string ModuleNo { get; set; }

        [StringLength(MaxCaseNoLength)]
        public virtual string CaseNo { get; set; }

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxRenbanLength)]
        public virtual string Renban { get; set; }

        public virtual long? InvoiceId { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual long? ContainerListId { get; set; }

        [StringLength(MaxModuleCaseNoLength)]
        public virtual string ModuleCaseNo { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        [StringLength(MaxStorageLocationCodeLength)]
        public virtual string StorageLocationCode { get; set; }

        public virtual long? LocationId { get; set; }

        public virtual DateTime? LocationDate { get; set; }

        [StringLength(MaxLocationByLength)]
        public virtual string LocationBy { get; set; }

        public virtual DateTime? UnpackingDate { get; set; }

        public virtual string UnpackingTime { get; set; }

        public virtual DateTime? UnpackingDatetime { get; set; }

        public virtual decimal? NetWeight { get; set; }

        public virtual decimal? GrossWeight { get; set; }

        public virtual int? MeasuarementM3 { get; set; }

        public virtual int? Lengthofcasemodule { get; set; }

        public virtual int? Widthofcasemodule { get; set; }

        public virtual int? Heightofcasemodule { get; set; }

        [StringLength(MaxDummyLength)]
        public virtual string Dummy { get; set; }

        public virtual DateTime? Plannedpackingdate { get; set; }

        [StringLength(MaxDgflagLength)]
        public virtual string Dgflag { get; set; }

        [StringLength(MaxPackingTypeLength)]
        public virtual string PackingType { get; set; }

        [StringLength(MaxRrTypeLength)]
        public virtual string RrType { get; set; }

        public virtual decimal? FreightPerInvoice { get; set; }

        public virtual decimal? InsurancePerInvoice { get; set; }

        [StringLength(MaxInnerMaterialType1Length)]
        public virtual string InnerMaterialType1 { get; set; }

        [StringLength(MaxInnerMaterialType2Length)]
        public virtual string InnerMaterialType2 { get; set; }

        [StringLength(MaxInnerMaterialType3Length)]
        public virtual string InnerMaterialType3 { get; set; }

        [StringLength(MaxInnerMaterialType4Length)]
        public virtual string InnerMaterialType4 { get; set; }

        [StringLength(MaxInnerMaterialType5Length)]
        public virtual string InnerMaterialType5 { get; set; }

        [StringLength(MaxInnerMaterialType6Length)]
        public virtual string InnerMaterialType6 { get; set; }

        [StringLength(MaxInnerMaterialType7Length)]
        public virtual string InnerMaterialType7 { get; set; }

        [StringLength(MaxInnerMaterialType8Length)]
        public virtual string InnerMaterialType8 { get; set; }

        [StringLength(MaxInnerMaterialType9Length)]
        public virtual string InnerMaterialType9 { get; set; }

        [StringLength(MaxInnerMaterialType10Length)]
        public virtual string InnerMaterialType10 { get; set; }

        [StringLength(MaxInnerMaterialType11Length)]
        public virtual string InnerMaterialType11 { get; set; }

        [StringLength(MaxInnerMaterialType12Length)]
        public virtual string InnerMaterialType12 { get; set; }

        public virtual int? InnerMaterialQuantity1 { get; set; }

        public virtual int? InnerMaterialQuantity2 { get; set; }

        public virtual int? InnerMaterialQuantity3 { get; set; }

        public virtual int? InnerMaterialQuantity4 { get; set; }

        public virtual int? InnerMaterialQuantity5 { get; set; }

        public virtual int? InnerMaterialQuantity6 { get; set; }

        public virtual int? InnerMaterialQuantity7 { get; set; }

        public virtual int? InnerMaterialQuantity8 { get; set; }

        public virtual int? InnerMaterialQuantity9 { get; set; }

        public virtual int? InnerMaterialQuantity10 { get; set; }

        public virtual int? InnerMaterialQuantity11 { get; set; }

        public virtual int? InnerMaterialQuantity12 { get; set; }

        public virtual int? Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

    }

}

