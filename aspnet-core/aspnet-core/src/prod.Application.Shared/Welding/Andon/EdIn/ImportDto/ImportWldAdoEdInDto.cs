using Abp.Application.Services.Dto;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using prod.Master.LogA;

namespace prod.Welding.Andon.ImportDto
{
	public class ImportWldAdoEdInDto : EntityDto<long?>
    {
        [StringLength(MstLgaBp2PartListConsts.MaxGuidLength)]
        public virtual string Guid { get; set; }

        [Required]
        public virtual int? No { get; set; }

        [Required]
        public virtual int? Seq { get; set; }

        [StringLength(WldAdoEdInConsts.MaxModelLength)]
        public virtual string Model { get; set; }

        [Required]
        [StringLength(WldAdoEdInConsts.MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        [Required]
        public virtual int? NoInLot { get; set; }

        [Required]
        [StringLength(WldAdoEdInConsts.MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        [StringLength(WldAdoEdInConsts.MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(WldAdoEdInConsts.MaxVinLength)]
        public virtual string Vin { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? RequestDate { get; set; }

        [StringLength(WldAdoEdInConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(WldAdoEdInConsts.MaxPlanTimeLength)]
        public virtual string PlanTime { get; set; }

        [StringLength(WldAdoEdInConsts.MaxShiftLength)]
        public virtual string Shift { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? EdIn { get; set; }

        [StringLength(WldAdoEdInConsts.MaxRemarksLength)]
        public virtual string Remarks { get; set; }

        public string Exception { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }

}

