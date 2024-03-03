using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{
    public class MstLgaEkbPartListDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string PartNoNormanlized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Model { get; set; }

        public virtual long? ProcessId { get; set; }

        public virtual string ProcessCode { get; set; }

        public virtual string ModuleCode { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string ExporterBackNo { get; set; }

        public virtual string BodyColor { get; set; }

        public virtual string Grade { get; set; }

        public virtual int? UsageQty { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string PcAddress { get; set; }

        public virtual int? PcSorting { get; set; }

        public virtual string SpsAddress { get; set; }

        public virtual int? SpsSorting { get; set; }

        public virtual string Remark { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstLgaEkbPartListDto : EntityDto<long?>
    {

        [StringLength(MstLgaEkbPartListConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxPartNoNormanlizedLength)]
        public virtual string PartNoNormanlized { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxModelLength)]
        public virtual string Model { get; set; }

        public virtual long? ProcessId { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxProcessCodeLength)]
        public virtual string ProcessCode { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxModuleCodeLength)]
        public virtual string ModuleCode { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxExporterBackNoLength)]
        public virtual string ExporterBackNo { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxBodyColorLength)]
        public virtual string BodyColor { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual int? UsageQty { get; set; }

        public virtual int? BoxQty { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxPcAddressLength)]
        public virtual string PcAddress { get; set; }

        public virtual int? PcSorting { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxSpsAddressLength)]
        public virtual string SpsAddress { get; set; }

        public virtual int? SpsSorting { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(MstLgaEkbPartListConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstLgaEkbPartListInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Model { get; set; }

        public virtual string ProcessCode { get; set; }
    }

    public class GetMstLgaEkbPartListExcelInput
    {

        public virtual string PartNo { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Model { get; set; }

        public virtual string ProcessCode { get; set; }
    }
}

