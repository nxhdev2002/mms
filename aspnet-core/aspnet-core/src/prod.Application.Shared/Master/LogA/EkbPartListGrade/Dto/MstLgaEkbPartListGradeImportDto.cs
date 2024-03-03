using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.LogA.Dto
{
    public class MstLgaEkbPartListGradeImportDto : EntityDto<long?>
    {
        [StringLength(MstLgaEkbPartListGradeConsts.MaxGuidLength)]
        public virtual string Guid { get; set; }

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

        [StringLength(MstLgaEkbPartListGradeConsts.MaxExporterBackNoLength)]
        public virtual string ExporterBackNo { get; set; } 

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

        [StringLength(MstLgaEkbPartListGradeConsts.MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxBodyColorLength)]
        public virtual string BodyColor { get; set; }

        //
        [StringLength(MstLgaEkbPartListGradeConsts.MaxNameScreenLength)]
        public virtual string NameScreen { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxPcRackAddressLength)]
        public virtual string PcRackAddress { get; set; }

        public virtual int? UsagePerHour { get; set; }

        public virtual int? RackCapBox { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxOutTypeLength)]
        public virtual string OutType { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxStockQtyLength)]
        public virtual string StockQty { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxSpsRackAddressLength)]
        public virtual string SpsRackAddress { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxPcPickingMemberLength)]
        public virtual string PcPickingMember { get; set; }
        public virtual int? EkbQty { get; set; }

        [StringLength(MstLgaEkbPartListGradeConsts.MaxProcessLength)]
        public virtual string Process { get; set; }

        public string Exception { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}
