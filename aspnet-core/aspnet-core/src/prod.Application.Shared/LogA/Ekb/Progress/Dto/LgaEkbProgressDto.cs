using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogA.Ekb.Dto
{

    public class LgaEkbProgressDto : EntityDto<long?>
    {

        public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual string Shift { get; set; }

        public virtual int? NoInShift { get; set; }

        public virtual int? NoInDate { get; set; }

        public virtual long? ProcessId { get; set; }

        public virtual string ProcessCode { get; set; }

        public virtual DateTime? NewtaktDatetime { get; set; }

        public virtual DateTime? StartDatetime { get; set; }

        public virtual DateTime? FinishDatetime { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditLgaEkbProgressDto : EntityDto<long?>
    {

        [StringLength(LgaEkbProgressConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(LgaEkbProgressConsts.MaxShiftLength)]
        public virtual string Shift { get; set; }

        public virtual int? NoInShift { get; set; }

        public virtual int? NoInDate { get; set; }

        public virtual long? ProcessId { get; set; }

        [StringLength(LgaEkbProgressConsts.MaxProcessCodeLength)]
        public virtual string ProcessCode { get; set; }

        public virtual DateTime? NewtaktDatetime { get; set; }

        public virtual DateTime? StartDatetime { get; set; }

        public virtual DateTime? FinishDatetime { get; set; }

        [StringLength(LgaEkbProgressConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(LgaEkbProgressConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetLgaEkbProgressInput : PagedAndSortedResultRequestDto
    {
        public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }


        public virtual string Shift { get; set; }

        public virtual string ProcessCode { get; set; }

        public virtual DateTime? StartDatetime { get; set; }

        public virtual DateTime? FinishDatetime { get; set; }
    }

    public class GetLgaEkbProgressExportInput
    {
        public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

        public virtual string ProcessCode { get; set; }

        public virtual DateTime? StartDatetime { get; set; }

        public virtual DateTime? FinishDatetime { get; set; }
    }
}

