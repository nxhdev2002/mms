using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.LogA.Dto
{
    public class MstLgaEkbPartListImportDto : EntityDto<long?>
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

        public string Exception { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}
