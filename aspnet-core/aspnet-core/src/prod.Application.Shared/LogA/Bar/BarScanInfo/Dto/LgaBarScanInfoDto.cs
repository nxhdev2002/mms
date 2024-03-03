using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogA.Bar.Dto
{
    public class LgaBarScanInfoDto : EntityDto<long?>
    {
        public virtual string UserId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string ScanValue { get; set; }

        public virtual string ScanPartNo { get; set; }

        public virtual string ScanBackNo { get; set; }

        public virtual string ScanType { get; set; }

        public virtual DateTime? ScanDatetime { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual long? RefId { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditLgaBarScanInfoDto : EntityDto<long?>
    {
        [StringLength(LgaBarScanInfoConsts.MaxUserIdLength)]
        public virtual string UserId { get; set; }

        [StringLength(LgaBarScanInfoConsts.MaxUserNameLength)]
        public virtual string UserName { get; set; }

        [StringLength(LgaBarScanInfoConsts.MaxScanValueLength)]
        public virtual string ScanValue { get; set; }

        [StringLength(LgaBarScanInfoConsts.MaxScanPartNoLength)]
        public virtual string ScanPartNo { get; set; }

        [StringLength(LgaBarScanInfoConsts.MaxScanBackNoLength)]
        public virtual string ScanBackNo { get; set; }

        [StringLength(LgaBarScanInfoConsts.MaxScanTypeLength)]
        public virtual string ScanType { get; set; }

        public virtual DateTime? ScanDatetime { get; set; }

        [StringLength(LgaBarScanInfoConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        public virtual long? RefId { get; set; }

        [StringLength(LgaBarScanInfoConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(LgaBarScanInfoConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetLgaBarScanInfoInput : PagedAndSortedResultRequestDto
    {
        public virtual string UserName { get; set; }
        public virtual string ScanPartNo { get; set; }
        public virtual string ProdLine { get; set; }
    }

    public class GetLgaBarScanInfoExportInput
    { 
        public virtual string UserName { get; set; }
        public virtual string ScanPartNo { get; set; }
        public virtual string ProdLine { get; set; }
    }
}

