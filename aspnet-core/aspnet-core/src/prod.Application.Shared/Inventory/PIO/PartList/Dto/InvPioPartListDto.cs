using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.PIO.PartList.Dto
{
    public class InvPioPartListDto : EntityDto<long?>
    {

        public virtual string FullModel { get; set; }

        public virtual string ProdSfx { get; set; }

        public virtual string MktCode { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartType { get; set; }

        public virtual string PartDescription { get; set; }

        public virtual string PioType { get; set; }

        public virtual int? BoxSize { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual string Supplier { get; set; }

        public virtual string Remark { get; set; }

        public virtual string IsActive { get; set; }

        public virtual string Cfc { get; set; }
        public virtual string Model { get; set; }
        public virtual string Grade { get; set; }
        public virtual string MaterialId { get; set; }
    }

    public class CreateOrEditInvPioPartListDto : EntityDto<long?>
    {

        [StringLength(InvPioPartListConsts.MaxFullModelLength)]
        public virtual string FullModel { get; set; }

        [StringLength(InvPioPartListConsts.MaxProdSfxLength)]
        public virtual string ProdSfx { get; set; }

        [StringLength(InvPioPartListConsts.MaxMktCodeLength)]
        public virtual string MktCode { get; set; }

        [StringLength(InvPioPartListConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(InvPioPartListConsts.MaxPartNoLength)]
        public virtual string PartName { get; set; }

        [StringLength(InvPioPartListConsts.MaxPartTypeLength)]
        public virtual string PartType { get; set; }

        [StringLength(InvPioPartListConsts.MaxPioTypeLength)]
        public virtual string PioType { get; set; }

        public virtual int? BoxSize { get; set; }

        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

        [StringLength(InvPioPartListConsts.MaxSupplierLength)]
        public virtual string Supplier { get; set; }

        //[StringLength(InvPioPartListConsts.MaxPartDescriptionLength)]
        //public virtual string PartDescription { get; set; }

        [StringLength(InvPioPartListConsts.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(InvPioPartListConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
        [StringLength(InvPioPartListConsts.MaxCfcLength)]

        public virtual string Cfc { get; set; }
        [StringLength(InvPioPartListConsts.MaxModelLength)]
        public virtual string Model { get; set; }
        public virtual string Grade { get; set; }
        public virtual string MaterialId { get; set; }
    }
    public class GetInvPioPartListHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
    public class ChangedRecordIdsInvPioPartListDto
    {
        public virtual List<long?> PartList { get; set; }
    }

    public class GetInvPioPartListInput : PagedAndSortedResultRequestDto
    {
        public virtual string FullModel { get; set; }
        public virtual string MktCode { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string IsActive { get; set; }

    }

    public class InvPioPartListImportDto
    {
        public virtual long? ROW_NO { get; set; }
        public virtual string Type { get; set; }
        public virtual string Model { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string ECIPart { get; set; }
        public virtual int? BoxSize { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual string MktCode { get; set; }
        public virtual int? Qty { get; set; }
        public virtual string Guid { get; set; }
        public virtual string ErrorDescription { get; set; }
    }

}
