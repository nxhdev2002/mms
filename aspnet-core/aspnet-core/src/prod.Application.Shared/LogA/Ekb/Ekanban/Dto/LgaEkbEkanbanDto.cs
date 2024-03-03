using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;

namespace prod.LogA.Ekb.Ekanban.Dto
{
    public class LgaEkbEkanbanDto : EntityDto<long?>
    {

        public virtual string KanbanSeq { get; set; }

        public virtual int? KanbanNoInDate { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual string Shift { get; set; }

        public virtual long? ProgressId { get; set; }

        public virtual long? ProcessId { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string PcAddress { get; set; }

        public virtual string SpsAddress { get; set; }

        public virtual int? Sorting { get; set; }

        public virtual int? RequestQty { get; set; }

        public virtual int? ScanQty { get; set; }

        public virtual int? InputQty { get; set; }

        public virtual string IsZeroKb { get; set; }

        public virtual DateTime? RequestDatetime { get; set; }

        public virtual DateTime? PikStartDatetime { get; set; }

        public virtual DateTime? PikFinishDatetime { get; set; }

        public virtual DateTime? DelStartDatetime { get; set; }

        public virtual DateTime? DelFinishDatetime { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsActive { get; set; }
    }

    public class CreateOrEditLgaEkbEkanbanDto : EntityDto<long?>
    {

        [StringLength(LgaEkbEkanbanConsts.MaxKanbanSeqLength)]
        public virtual string KanbanSeq { get; set; }

        public virtual int? KanbanNoInDate { get; set; }

        [StringLength(LgaEkbEkanbanConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(LgaEkbEkanbanConsts.MaxShiftLength)]
        public virtual string Shift { get; set; }

        public virtual long? ProgressId { get; set; }

        public virtual long? ProcessId { get; set; }

        public virtual long? PartListId { get; set; }

        [StringLength(LgaEkbEkanbanConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(LgaEkbEkanbanConsts.MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(LgaEkbEkanbanConsts.MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(LgaEkbEkanbanConsts.MaxPcAddressLength)]
        public virtual string PcAddress { get; set; }

        [StringLength(LgaEkbEkanbanConsts.MaxSpsAddressLength)]
        public virtual string SpsAddress { get; set; }

        public virtual int? Sorting { get; set; }

        public virtual int? RequestQty { get; set; }

        public virtual int? ScanQty { get; set; }

        public virtual int? InputQty { get; set; }

        [StringLength(LgaEkbEkanbanConsts.MaxIsZeroKbLength)]
        public virtual string IsZeroKb { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? RequestDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? PikStartDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? PikFinishDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? DelStartDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? DelFinishDatetime { get; set; }

        [StringLength(LgaEkbEkanbanConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(LgaEkbEkanbanConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetLgaEkbEkanbanInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string PcAddress { get; set; }

        public virtual string SpsAddress { get; set; }

    }

    public class GetLgaEkbEkanbanExportInput
    {
        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string PcAddress { get; set; }

        public virtual string SpsAddress { get; set; }

    }
}
