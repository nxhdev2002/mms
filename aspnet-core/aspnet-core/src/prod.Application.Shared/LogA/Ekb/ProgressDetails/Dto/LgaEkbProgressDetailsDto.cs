using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogA.Ekb.Dto
{

    public class LgaEkbProgressDetailsDto : EntityDto<long?>
    {

        public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual string Shift { get; set; }

        public virtual int? NoInShift { get; set; }

        public virtual int? NoInDate { get; set; }

        public virtual long? ProgressId { get; set; }

        public virtual long? ProcessId { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string PcAddress { get; set; }

        public virtual string SpsAddress { get; set; }

        public virtual int? Sorting { get; set; }

        public virtual int? UsageQty { get; set; }
        
        public virtual string SequenceNo { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string Grade { get; set; }

        public virtual string Model { get; set; }

        public virtual string BodyColor { get; set; }

        public virtual int? EkbQty { get; set; }

        public virtual int? RemainQty { get; set; }

        public virtual string IsZeroKb { get; set; }

        public virtual DateTime? NewtaktDatetime { get; set; }

        public virtual DateTime? PikStartDatetime { get; set; }

        public virtual DateTime? PikFinishDatetime { get; set; }

        public virtual DateTime? DelStartDatetime { get; set; }

        public virtual DateTime? DelFinishDatetime { get; set; }

        public virtual long? KanbanId { get; set; }

        public virtual string KanbanSeq { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditLgaEkbProgressDetailsDto : EntityDto<long?>
    {

        [StringLength(LgaEkbProgressDetailsConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxShiftLength)]
        public virtual string Shift { get; set; }

        public virtual int? NoInShift { get; set; }

        public virtual int? NoInDate { get; set; }

        public virtual long? ProgressId { get; set; }

        public virtual long? ProcessId { get; set; }

        public virtual long? PartListId { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxPcAddressLength)]
        public virtual string PcAddress { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxSpsAddressLength)]
        public virtual string SpsAddress { get; set; }

        public virtual int? Sorting { get; set; }

        public virtual int? UsageQty { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxSequenceNoLength)]
        public virtual string SequenceNo { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxBodyColorLength)]
        public virtual string BodyColor { get; set; }

        public virtual int? EkbQty { get; set; }

        public virtual int? RemainQty { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxIsZeroKbLength)]
        public virtual string IsZeroKb { get; set; }

        public virtual DateTime? NewtaktDatetime { get; set; }

        public virtual DateTime? PikStartDatetime { get; set; }

        public virtual DateTime? PikFinishDatetime { get; set; }

        public virtual DateTime? DelStartDatetime { get; set; }

        public virtual DateTime? DelFinishDatetime { get; set; }

        public virtual int? KanbanId { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxKanbanSeqLength)]
        public virtual string KanbanSeq { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(LgaEkbProgressDetailsConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetLgaEkbProgressDetailsInput : PagedAndSortedResultRequestDto
    {
        public virtual string ProdLine { get; set; }

        public virtual string Shift { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Grade { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string KanbanSeq { get; set; }

        public virtual string SequenceNo { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }
    }

}


