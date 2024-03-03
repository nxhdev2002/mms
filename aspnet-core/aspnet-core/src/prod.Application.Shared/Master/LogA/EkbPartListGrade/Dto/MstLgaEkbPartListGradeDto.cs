using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{
    public class MstLgaEkbPartListGradeDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string PartNoNormanlized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string BackNo { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Model { get; set; }

        public virtual long? ProcessId { get; set; }

        public virtual string ProcessCode { get; set; }

        public virtual string Grade { get; set; }

        public virtual int? UsageQty { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string Module { get; set; }

        public virtual string PcAddress { get; set; }

        public virtual int? PcSorting { get; set; }

        public virtual string SpsAddress { get; set; }

        public virtual int? SpsSorting { get; set; }

        public virtual string Remark { get; set; }

        public virtual string IsActive { get; set; }

        public int A1 { get; set; }
        public int A2 { get; set; }
        public int A3 { get; set; }
        public int A4 { get; set; }
        public int A5 { get; set; }
        public int A6 { get; set; }
        public int A7 { get; set; }
        public int A8 { get; set; }
        public int A9 { get; set; }
        public int A10 { get; set; }

    }

    public class CreateOrEditMstLgaEkbPartListGradeDto : EntityDto<long?>
    {

        [StringLength(MstLgaEkbPartListGradeConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxPartNoNormanlizedLength)]
        public virtual string PartNoNormanlized { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        public virtual long? PartListId { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxModelLength)]
        public virtual string Model { get; set; }

        public virtual long? ProcessId { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxProcessCodeLength)]
        public virtual string ProcessCode { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual int? UsageQty { get; set; }

        public virtual int? BoxQty { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxModuleLength)]
        public virtual string Module { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxPcAddressLength)]
        public virtual string PcAddress { get; set; }

        public virtual int? PcSorting { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxSpsAddressLength)]
        public virtual string SpsAddress { get; set; }

        public virtual int? SpsSorting { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstLgaEkbPartListGradeInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Model { get; set; }

        public virtual string ProcessCode { get; set; }

    }
}


