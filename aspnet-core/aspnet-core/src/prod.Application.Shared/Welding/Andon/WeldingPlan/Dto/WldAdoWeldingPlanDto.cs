using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Welding.Andon.Dto
{

	public class WldAdoWeldingPlanDto : EntityDto<long?>
	{
        public virtual string Model { get; set; }

		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

        public virtual string Grade { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string Color { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string PlanTime { get; set; }

		public virtual DateTime? RequestDate { get; set; }

		public virtual string Shift { get; set; }

		public virtual DateTime? WInDate { get; set; }

        public virtual string FormatWInDate
        {
            get
            {
                return WInDate == null ? "" : string.Format("{0:dd/MM/yyyy}", WInDate);
            }
            set { }
        }

        public virtual DateTime? WOutDate { get; set; }

        public virtual string FormatWOutDate
        {
            get
            {
                return WOutDate == null ? "" : string.Format("{0:dd/MM/yyyy}", WOutDate);
            }
            set { }
        }

        public virtual DateTime? EdIn { get; set; }

        public virtual string FormatEdIn
        {
            get
            {
                return EdIn == null ? "" : string.Format("{0:dd/MM/yyyy}", EdIn);
            }
            set { }
        }

        public virtual DateTime? TInPlanDatetime { get; set; }

        public virtual long? VehicleId { get; set; }

        public virtual string Cfc { get; set; }

        public virtual long? WeldingId { get; set; }

        public virtual long? AssemblyId { get; set; }

        public virtual string MessagesError { get; set; }

    }

	public class CreateOrEditWldAdoWeldingPlanDto : EntityDto<long?>
	{

        [Required]
		[StringLength(WldAdoWeldingPlanConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[Required]
		[StringLength(WldAdoWeldingPlanConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

        [StringLength(WldAdoWeldingPlanConsts.MaxLotNoLength)]
        public virtual string ProdLine { get; set; }

        [Required]
		public virtual int? NoInLot { get; set; }

        [StringLength(WldAdoWeldingPlanConsts.MaxGradeLength)]
        public virtual string Grade { get; set; }

        [Required]
		[StringLength(WldAdoWeldingPlanConsts.MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

        [StringLength(WldAdoWeldingPlanConsts.MaxVinNoLength)]
        public virtual string VinNo { get; set; }

        [StringLength(WldAdoWeldingPlanConsts.MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(WldAdoWeldingPlanConsts.MaxPlanTimeLength)]
		public virtual string PlanTime { get; set; }

		public virtual DateTime? RequestDate { get; set; }

		[StringLength(WldAdoWeldingPlanConsts.MaxShiftLength)]
		public virtual string Shift { get; set; }

		public virtual DateTime? WInDate { get; set; }

		public virtual DateTime? WOutDate { get; set; }

		public virtual DateTime? EdIn { get; set; }

        public virtual DateTime? TInPlanDatetime { get; set; }

        public virtual long? VehicleId { get; set; }

        [StringLength(WldAdoWeldingPlanConsts.MaxCfcLength)]
        public virtual string Cfc { get; set; }

        public virtual long? WeldingId { get; set; }

        public virtual long? AssemblyId { get; set; }
	}

	public class GetWldAdoWeldingPlanInput : PagedAndSortedResultRequestDto
	{
        public virtual string Model { get; set; }

		public virtual string LotNo { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string VinNo { get; set; }

        public virtual DateTime? RequestDateFrom { get; set; }

        public virtual DateTime? RequestDateTo { get; set; }

    }

    public class GetWldAdoWeldingPlanExportInput 
    {
        public virtual string Model { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string VinNo { get; set; }

        public virtual DateTime? RequestDateFrom { get; set; }

        public virtual DateTime? RequestDateTo { get; set; }

    }
    public class GetWldAdoWeldingPlanHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }

    public class GetWldAdoWeldingPlanHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
}

